# 2.2.2 `NetRandom`

[Parent: 2.2 RandomLib: random sources](random-sources.md) · [Guide map](../index.md)

`NetRandom` wraps `System.Random`. It accepts an optional numeric seed or an `ISeedR`. Its `NextLong` uses random bytes to sample the requested range.

```csharp
using RandEXom.RandomLib;

var random = new NetRandom(seed: 42);
int index = random.NextInt(0, 5);
```

`NetRandom` converts its seed to a 32-bit value for `System.Random`; different 64-bit seeds can therefore map to the same internal seed. For reproducibility, also keep the same target runtime: `System.Random` is a .NET implementation detail, not a cross-runtime sequence contract.

See the [Microsoft `System.Random` documentation](https://learn.microsoft.com/dotnet/api/system.random) for platform behavior. This wrapper should not be described as implementing a *Numerical Recipes* generator.

---

← Previous: [2.2.1 ModuloRandom](random-sources-modulorandom.md) · [Parent: 2.2 RandomLib: random sources](random-sources.md) · Next: [2.2.3 SSRNGRandom](random-sources-ssrngrandom.md) →
