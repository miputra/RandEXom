# 2.1.1 `XORShift64Seed`

[Parent: 2.1 SeedLib: seed generators](seed-generators.md) · [Guide map](../index.md)

This is `ModuloRandom`'s default seed generator. Choose the standard xorshift64 option, the `Xorshift64_star` option, or custom shift counts. A zero initial state is replaced internally with a nonzero state, while `init` still reports the supplied zero.

The xorshift family comes from George Marsaglia’s [“Xorshift RNGs” (2003)](https://www.jstatsoft.org/article/view/v008i14). The `Xorshift64_star` option matches the right, left, right shifts (12, 25, 27) in Figure 10 of Sebastiano Vigna’s [paper on scrambled xorshift generators](https://vigna.di.unimi.it/ftp/papers/xorshift.pdf). `ModuloRandom` supplies the paper’s multiplier for its `XORShift64_Star` option, scrambling output without changing the seed generator’s state. See [research and provenance](research.md). The paper tests the raw generator, not RandEXom’s bounded-number or byte APIs.

```csharp
using RandEXom.SeedLib;
using RandEXom.RandomLib;

var seed = new XORShift64Seed(XORShift64Seed.Type.Xorshift64_star, seed: 42);
var random = new ModuloRandom(seed, ModuloRandom.Multiplier.XORShift64_Star);
```

Custom shifts use `new XORShift64Seed(shift1, shift2, shift3, seed: 42)`; each shift must be from 1 through 63. Custom parameters are not automatically vetted for statistical quality.

---

← Previous: [2.1 SeedLib: seed generators](seed-generators.md) · [Parent: 2.1 SeedLib: seed generators](seed-generators.md) · Next: [2.1.2 LCGSeedR](seed-generators-lcgseedr.md) →
