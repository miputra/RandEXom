# 4.2 Interfaces, extension, and limitations

Chapter 4: Reference · Section 2 · [Guide map](../index.md)

RandEXom separates a random source (`IRandomR`) from a seed generator (`ISeedR`). Most Boolean, item, and number helpers accept an `IRandomR`, so you can choose one source and reuse it across helpers. Be aware that sharing a source shares its mutable state: a call from one helper changes the sequence seen by the next.

<a id="pass-a-custom-seed-progression"></a>

## 4.2.1 Pass a custom seed progression

The easiest extension point is `IterativeSeedRCustom`, which implements `ISeedR` from a function:

```csharp
using RandEXom.RandomLib;
using RandEXom.SeedLib;

var seed = new IterativeSeedRCustom(
    current => unchecked(current * 17 + 3),
    seed: 42);
var random = new ModuloRandom(seed);
int value = random.NextInt(0, 100);
```

The example shows the extension mechanism, not a recommended statistical algorithm. A custom function with a short cycle or constant output can make draws poor or even prevent useful progression.

<a id="implement-an-interface"></a>

## 4.2.2 Implement an interface

An `ISeedR` implementation must expose `init`, `now`, `previous`, and `Next()`. An `IRandomR` implementation must provide `NextInt`, `NextLong`, `NextBytes`, `GetSeed`, and `GetSeedSTR`. Follow the range contract—minimum inclusive, maximum exclusive—and reject empty or reversed ranges. Advance state consistently when generating values and bytes.

For example, a custom source can be passed directly to a helper:

```csharp
using RandEXom.Framework.Boolean;
using RandEXom.Interface;
using RandEXom.RandomLib;

IRandomR source = new ModuloRandom(seed: 42);
var flag = new TruePercentageR(source, percentage: 10f);
```

<a id="important-limits"></a>

## 4.2.3 Important limits

- RandEXom's generators are not cryptographic random-number generators. Do not use them for keys, credentials, tokens, or other security-sensitive values.
- Reproducibility depends on the generator, seed, library version, and—in the case of `NetRandom`—the .NET runtime. v1.5b fixed several generator and range calculations, changing some seeded sequences.
- Helper instances and random sources are mutable. Do not assume they are safe for concurrent use without your own synchronization.
- `GetSeedSTR()` reports the initial seed, not enough information to resume a generator from an arbitrary point.
- For `[min, max)`, `min` must be less than `max`; `max` itself cannot be returned. Use `NextInt(1, 7)` for a six-sided die.
- `GachaR<T>` and `GachaRBatched<T>` can return `default(T)` when empty, which may be indistinguishable from a valid item. Check the remaining count if that matters.
- `DistributedTreeR<T>` divides items once at initialization. Removal locks empty endpoints without redistributing items; check `Count()` before a pull when a stored default value is possible. See [the item-tree guide](items-distributedtreer.md).

<a id="verification"></a>

## 4.2.4 Verification

Run `dotnet run --project Verification/Verification.csproj` from the repository root. If you change a generator or helper, add cases there for fixed-seed sequences, extreme bounds, empty pools, and invalid arguments.

Use [TestOut](test-output.md) for interactive examples and [the benchmark runner](benchmarks.md) for measured performance and randomness diagnostics. They serve different purposes from correctness verification.

---

← Previous: [4.1.2 Internal: TypeR.RoundLongToInt(long)](utility-typer.md) · [Guide map](../index.md) · Next: [4.3 Research and provenance](research.md) →
