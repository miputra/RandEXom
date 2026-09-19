# 4.2 Interfaces, extension, and limitations

Chapter 4: Reference · Page 2 of 4 · [Guide map](../index.md)

RandEXom separates a random source (`IRandomR`) from a seed generator (`ISeedR`). Most Boolean, item, and number helpers accept an `IRandomR`, so you can choose one source and reuse it across helpers. Be aware that sharing a source shares its mutable state: a call from one helper changes the sequence seen by the next.

## Pass a custom seed progression

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

## Implement an interface

An `ISeedR` implementation must expose `init`, `now`, `previous`, and `Next()`. An `IRandomR` implementation must provide `NextInt`, `NextLong`, `NextBytes`, `GetSeed`, and `GetSeedSTR`. Follow the range contract—minimum inclusive, maximum exclusive—and reject empty or reversed ranges. Advance state consistently when generating values and bytes.

For example, a custom source can be passed directly to a helper:

```csharp
using RandEXom.Framework.Boolean;
using RandEXom.Interface;
using RandEXom.RandomLib;

IRandomR source = new ModuloRandom(seed: 42);
var flag = new TruePercentageR(source, percentage: 10f);
```

## Important limits

- RandEXom's generators are not cryptographic random-number generators. Do not use them for keys, credentials, tokens, or other security-sensitive values.
- Reproducibility depends on the generator, seed, library version, and—in the case of `NetRandom`—the .NET runtime. v1.5b fixed several generator and range calculations, changing some seeded sequences.
- Helper instances and random sources are mutable. Do not assume they are safe for concurrent use without your own synchronization.
- `GetSeedSTR()` reports the initial seed, not enough information to resume a generator from an arbitrary point.
- For `[min, max)`, `min` must be less than `max`; `max` itself cannot be returned. Use `NextInt(1, 7)` for a six-sided die.
- `GachaR<T>` and `GachaRBatched<T>` can return `default(T)` when empty, which may be indistinguishable from a valid item. Check the remaining count if that matters.
- The distributed-tree types are obsolete/experimental, and dice/noise types are debug-only placeholders. See [number helpers](numbers.md).

## Verification

Run `dotnet run --project Verification/Verification.csproj` from the repository root. If you change a generator or helper, add cases there for fixed-seed sequences, extreme bounds, empty pools, and invalid arguments.

---

← Previous: [4.1 Utility](utility.md) · [Guide map](../index.md) · Next: [4.3 Research and provenance](research.md) →
