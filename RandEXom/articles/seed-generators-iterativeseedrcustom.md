# 2.1.4 `IterativeSeedRCustom`

[Parent: 2.1 SeedLib: seed generators](seed-generators.md) · [Guide map](../index.md)

Provide a function that maps the current state to the next one:

```csharp
using RandEXom.SeedLib;
using RandEXom.RandomLib;

var seed = new IterativeSeedRCustom(current => unchecked(current * 17 + 3), seed: 42);
var random = new ModuloRandom(seed);
```

The function cannot be `null`. Avoid functions that immediately converge to a constant or cycle over a tiny set if you need varied output.

---

← Previous: [2.1.3 IterativeSeedR](seed-generators-iterativeseedr.md) · [Parent: 2.1 SeedLib: seed generators](seed-generators.md) · Next: [2.1.5 Constant seed (SeedR)](seed-generators-seedr.md) →
