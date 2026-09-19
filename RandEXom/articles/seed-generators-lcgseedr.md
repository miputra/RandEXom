# 2.1.2 `LCGSeedR`

[Parent: 2.1 SeedLib: seed generators](seed-generators.md) · [Guide map](../index.md)

`LCGSeedR` advances a linear congruential generator: its next state depends on a multiplier `a`, increment `c`, and modulus `m`. The default parameters are the *[Numerical Recipes in C](https://numerical.recipes/)* preset; that source is a **book**, not a paper. For research on LCG parameter quality and lattice structure, see Pierre L’Ecuyer’s [1999 paper](https://www.ams.org/mcom/1999-68-225/S0025-5718-99-00996-5/S0025-5718-99-00996-5.pdf). You may choose one of the `ParameterTemplate` values or supply parameters directly:

```csharp
using RandEXom.SeedLib;
using RandEXom.RandomLib;

var seed = new LCGSeedR(seed: 42, a: 1664525, c: 1013904223, m: 4294967296);
seed.Next();
long current = seed.now;
```

`m` must be greater than one. The available preset names are the same as those listed for `SSRNGRandom`; they are historical parameter sets and should not be treated as cryptographic options.

---

← Previous: [2.1.1 XORShift64Seed](seed-generators-xorshift64seed.md) · [Parent: 2.1 SeedLib: seed generators](seed-generators.md) · Next: [2.1.3 IterativeSeedR](seed-generators-iterativeseedr.md) →
