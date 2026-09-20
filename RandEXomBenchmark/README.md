# RandEXomBenchmark

A standalone benchmark runner that loads a DLL at runtime. It has **no RandEXom project reference and no NuGet dependencies**. The target DLL is never built or modified by the runner.

See [measured comparisons and archived reports](../RandEXom/articles/benchmarks.md) for the current documentation snapshot. [TestOut](../RandEXom/articles/test-output.md) is the separate interactive output demonstration.

## Use the executable

Put these two files in one folder:

```text
RandEXomBenchmark.exe
RandEXom.dll
```

The published Windows x64 executable requires the .NET 8 runtime, but no SDK, solution, or source files. A self-contained publishing option is below if the destination has no runtime. Other dependencies of the target DLL must also be beside that DLL.

Double-click `RandEXomBenchmark.exe` to open an interactive menu like TestOut. Choose a numbered option to list benchmarks, measure performance/memory, check randomness quality, run smoke checks, or export bytes. The menu stays open after each run, including failed runs; choose `0` to exit.

Use **Settings and injection** to change the DLL, random/seed implementations, constructor JSON file, type filter, item count, sample count, byte count, or report directory. Settings apply to later runs in that session. Enter keeps the current value; `-` clears an optional setting. The DLL beside the executable is selected by default.

Command-line use is still available and does not open the menu:

```powershell
.\RandEXomBenchmark.exe list
.\RandEXomBenchmark.exe performance
.\RandEXomBenchmark.exe quality
.\RandEXomBenchmark.exe smoke
```

Or point it at any compatible DLL:

```powershell
.\RandEXomBenchmark.exe performance --dll C:\Libraries\RandEXom.dll --random ModuloRandom --seed-type XORShift64Seed --samples 25
.\RandEXomBenchmark.exe performance --filter DistributedTree --items 1024 --config tree.json
.\RandEXomBenchmark.exe quality --bytes 1048576 --output results\quality-large
```

`tree.json` contains constructor argument values, for example:

```json
{ "step": 3, "child": 4, "do_shuffle": true }
```

The runner measures both keep/remove variants automatically when the constructor has `remove_on_pull`. Equal endpoint division happens only when constructing the target tree. It uses exactly the configured item count of pulls per sample, recreating a full instance outside the timed region, so empty-pool draws are never counted as removal work.

## Discovery and injection

The runner reads public concrete types and methods from the loaded assembly. One-parameter generic item classes are closed with `int`. Newly added classes with supported public shapes need no runner rebuild:

- `NextInt(int, int)`, `NextLong(long, long)`, or `Next` with the same ranges.
- Parameterless `Next`, `NextInt`, `NextLong`, and `Pull`.
- `NextBytes(byte[])` and `Shuffle<T>(List<T>)`.
- Parameterless `Count`, `CountType`, `ToList`, `GetSeed`, and `GetSeedSTR`.
- A constructible public constructor, measured separately with its injected dependencies.

Factories prefer constructor parameters supplied in JSON, then interface injection, then numeric seeds. Interface identity is taken from the loaded DLL, not from a compile-time reference. `--random` selects the implementation injected into framework constructors. Without it, the first assignable concrete implementation in full-name order is selected. `--seed-type` selects the implementation injected into random constructors where supported; otherwise numeric seed constructors are preferred. Use full names if simple names are ambiguous.

Numeric seed parameters named `seed` receive a fixed seed. Input item collections receive `0..ItemCount-1`; constructors should copy rather than mutate their input. Bags exposing `AddItem(int, int)` are filled before measurement. Optional parameters retain library defaults unless overridden with JSON. A required `percentage` float receives 25. JSON handles other required scalar values and enum names, allowing new constructor options without code changes. The runner controls `seed`, `items`, `remove_on_pull`, and `reset_on_empty`; these names cannot be overridden in JSON. Custom delegate constructors require a new adapter and are listed as unsupported.

`list` and report JSON show skipped classes/methods and reasons. Unsupported lifecycle mutations (for example arbitrary removal/refill APIs), abstract/debug-only placeholders, multiple generic parameters, and custom signatures are not silently treated as valid benchmarks. Obsolete classes in a supplied DLL are skipped by default; `--include-obsolete` opts in.

## Efficiency reports

`performance` produces JSON and CSV in `results` beside the executable (or `--output`). The report records DLL path, version, SHA-256, runtime, OS, architecture, selected injection types, constructor arguments, and sample counts.

Measurements use compiled delegates, avoiding reflection invocation and numeric-result boxing in the timed loop. Three warmup batches precede 15 measured samples by default. Each sample starts with a fresh target outside the timed region. Report columns include mean, median, minimum, standard deviation, standard error, calls/second, managed allocated bytes/call, and GC collection counts during measurement.

The unit is one method invocation: a draw, a complete shuffle, a 1,024-byte fill, or one construction. Multiply byte-fill calls/second by 1,024 for bytes/second. Constructor costs include injected dependencies and exclude delegate compilation. Draw costs exclude construction and pool filling. A removal draw is averaged across a full drain, including endpoint locking. Managed allocations are not peak process memory.

This is an in-process harness so the executable does not need an SDK or generated benchmark projects. Samples include delegate and loop overhead, do not subtract a baseline, and may be affected by JIT tiering, GC, or other processes. Inspect `MinSampleMs`: very short samples are noisy. Use Release publishing, no debugger, matching configurations, and an idle machine; repeat runs before drawing conclusions. `smoke` uses two samples and validates execution/statistical fixtures, not timing reliability.

## Randomness diagnostics

Any constructible class exposing all three random methods is discovered for quality analysis. Three seeds (1, 12345, 987654321) are checked using the configured seed injection and constructor parameters. Enum presets can be selected by name in JSON; the runner uses the chosen configuration, not every possible enum/source combination in one run.

| Metric | Interpretation under independent uniform output |
| --- | --- |
| Byte chi-square | 256 buckets; expected statistic about 255, not zero |
| Byte entropy | Approaches 8 bits/byte; flat histograms alone do not prove independence |
| One-bit fraction / monobit Z | Fraction near 0.5 and standardized imbalance near zero |
| Runs Z | Conditional standardized count of same-bit runs; MSB-first across byte boundaries |
| Lag-1 correlation | Adjacent byte Pearson correlation, usually near zero |
| Pair chi-square | High nibbles from disjoint adjacent byte pairs, 256 buckets |
| Int chi-square | Distribution of NextInt(0, 256) |
| Replay and bounds | Repeated seed reproduces bytes; int bounds and excluded long maximum respected |

Undefined statistics are null (blank in CSV). Byte and integer checks use separate fresh sources; bounded integer draws are interleaved with full-range long draws. JSON includes DLL identity and configuration. CSV is suitable for spreadsheets. Smoke fixtures check constant bits, alternating bits, and a flat histogram with strong serial correlation.

This initial battery is not every known randomness test, a full NIST STS implementation, or a quality certification. [NIST explains the limits of statistical testing](https://csrc.nist.gov/pubs/sp/800/22/r1/upd1/final). It reports diagnostic statistics, not p-values or an overall quality score. Seed progressions and frameworks with constraints/removal do not necessarily follow independent-uniform assumptions.

Export raw bytes for external batteries such as PractRand or adapters for TestU01/NIST STS:

```powershell
.\RandEXomBenchmark.exe export --source ModuloRandom --seed 12345 --bytes 10485760 --output sample.bin
```

Export creates a new file and never overwrites an existing one. External suites are not bundled; follow their input requirements. Only replay/range or execution failures cause a nonzero exit code, not statistical deviations.

## Build the runner once

Building the runner requires an SDK; running it does not. This command publishes only the executable and does not build RandEXom:

```powershell
dotnet publish RandEXomBenchmark/RandEXomBenchmark.csproj -c Release -r win-x64 --self-contained false -p:EnableSingleFileAnalyzer=false -o RandEXomBenchmark/bin/portable
```

For an executable including the runtime (larger, needs runtime-pack download at publish time):

```powershell
dotnet publish RandEXomBenchmark/RandEXomBenchmark.csproj -c Release -r win-x64 --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableSingleFileAnalyzer=false -o RandEXomBenchmark/bin/self-contained
```

No target assembly reference, class list, or SDK process exists in the runner. `Tests/Fixture.csproj` is a separate validation fixture with unrelated class names/interfaces; it is not shipped or compiled into the runner. Run `Tests/Check.ps1` after publishing to verify discovery, interface injection, JSON constructor values, and complete removal sampling against that DLL.
