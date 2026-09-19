# Boolean helpers

This branch of [Framework](framework.md) converts a bounded draw from `IRandomR` into a Boolean event. It contains one helper, `RandEXom.Framework.Boolean.TruePercentageR`, which returns `true` with the requested probability on each independent draw. It accepts a percentage from 0 through 100, a seed for repeatable tests, or an `IRandomR` implementation.

```csharp
using RandEXom.Framework.Boolean;

var succeeds = new TruePercentageR(seed: 42, percentage: 25f);
bool result = succeeds.Next();
```

`25f` means a 25% chance per call; it does **not** guarantee exactly 25 successes in 100 calls. Zero always returns `false`, and 100 always returns `true`. `NaN` and percentages outside 0–100 are rejected.

To share a random source with other helpers:

```csharp
using RandEXom.RandomLib;

var random = new ModuloRandom(seed: 42);
var succeeds = new TruePercentageR(random, percentage: 12.5f);
```

Sharing a source also shares its advancing state, so the order of calls across helpers affects the sequence. `Reset()` recalculates the probability threshold; it does not rewind the random source or balance past results. `CreateItems()` performs the same threshold calculation and usually need not be called directly.
