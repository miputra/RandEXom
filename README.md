# RandEXom

RandEXom is a C# randomization library for .NET Standard 2.1. It provides interchangeable random sources and seed generators, plus helpers for shuffling, weighted picks, percentages, and number ranges.

## Quick start

Download [RandEXom.dll v1.5b](https://miputra.github.io/RandEXom/downloads/RandEXom.dll) or get it from the [GitHub release](https://github.com/miputra/RandEXom/releases/tag/1.5b), then add the DLL as a reference to a compatible .NET project.

```csharp
using RandEXom.RandomLib;

var random = new ModuloRandom(seed: 12345);
int value = random.NextInt(0, 100); // 0 through 99
```

Random range methods include the minimum and exclude the maximum.

**Version note:** v1.5b fixes generator and range calculations. Seeded sequences may differ from earlier releases.

## Documentation

Read the [hosted documentation on GitHub Pages](https://miputra.github.io/RandEXom/guide/) for the styled sidebar, chapter navigation, and previous/next links. The Markdown sources below remain available in the repository.

Read [how RandEXom fits together](RandEXom/articles/architecture.md) first: `SeedLib` manages state → `RandomLib` turns state into draws → `Framework` uses draws for application behavior. `Utility` provides supporting functions; `Interface` defines the contracts between layers.

| Next step | Guide |
| --- | --- |
| Install and make a first draw | [Installation](RandEXom/articles/installation.md) · [Getting started](RandEXom/articles/intro.md) |
| Choose the seed progression and random source | [SeedLib](RandEXom/articles/seed-generators.md) · [RandomLib](RandEXom/articles/random-sources.md) |
| Apply task-specific behavior | [Framework overview](RandEXom/articles/framework.md) → [Boolean](RandEXom/articles/boolean.md) · [Item](RandEXom/articles/items.md) · [Number](RandEXom/articles/numbers.md) |
| Understand support and customization | [Utility](RandEXom/articles/utility.md) · [Interfaces, extension, and limitations](RandEXom/articles/extending.md) |
| Find the original research and parameter sources | [Research and provenance](RandEXom/articles/research.md) |
| Locate a particular class | [Class index](RandEXom/api/index.md) |

The [documentation home](RandEXom/index.md) is also included in the repository. The library targets .NET Standard 2.1; the verification project targets .NET 8. There is no published NuGet package listed here—use the release DLL or a source-project reference.

To rebuild the GitHub Pages guide after editing the Markdown sources, run `docfx build RandEXom/docfx.json` from the repository root and commit the updated `docs/guide` files. The repository's GitHub Pages site serves the `docs/` directory; `docs/.nojekyll` keeps DocFX assets intact.

## Verification

Run `dotnet run --project Verification/Verification.csproj` to check the published LCG sequence and the library's range, weighted draw, percentage, and distance edge cases.

## Output examples

Open `RandExOmTestOut.exe` for interactive demonstrations, or run `dotnet run --project RandExOmTestOut/RandExOmTestOut.csproj`. [The TestOut guide](RandEXom/articles/test-output.md) documents all nine menu options and the item-tree keep/remove output.

## Benchmarks

`RandEXomBenchmark` loads a DLL at runtime and discovers supported classes/methods, with configurable random/seed injection. It has no reference to the RandEXom project. Publish the runner once, then use the executable beside any compatible RandEXom.dll. See the [benchmark guide](RandEXomBenchmark/README.md) for publishing, coverage, and report interpretation.

Double-click the executable for a numbered menu like TestOut, or pass arguments from PowerShell as below.

See the [measured benchmark comparisons](RandEXom/articles/benchmarks.md) for timing, allocation, randomness diagnostics, run configuration, and downloadable reports from the current build.

```powershell
.\RandEXomBenchmark.exe performance --dll RandEXom.dll
.\RandEXomBenchmark.exe quality --dll RandEXom.dll
```

## Support the project

If RandEXom has been useful, you can support its development through [Trakteer](https://trakteer.id/miputra?quantity=1) or send crypto directly:

| Asset | Network | Address |
| --- | --- | --- |
| BNB | BNB Smart Chain (BEP-20) | `0xE122d7d44604d59b27Ca3FAA44Fc1Da94CE0aE03` |
| SOL | Solana | `C8A5J9w7UkXeLVeCWhpP47SPCPvnuS55RJWybzFhB3jF` |

Before sending, check both the network and the full address in your wallet. Crypto transfers to an incorrect address or network may not be recoverable. Donations are optional and do not affect the MIT license.
