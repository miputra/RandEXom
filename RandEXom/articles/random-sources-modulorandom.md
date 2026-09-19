# 2.2.1 `ModuloRandom`

[Parent: 2.2 RandomLib: random sources](random-sources.md) · [Guide map](../index.md)

`ModuloRandom` is the simple general-purpose choice in this library. By default it advances an `XORShift64Seed`; you can pass a seed number, an `ISeedR`, or a multiplier option. Its bounded methods use rejection sampling to avoid modulo bias.

```csharp
using RandEXom.RandomLib;

var regular = new ModuloRandom(seed: 42);
var star = new ModuloRandom(ModuloRandom.Multiplier.XORShift64_Star, seed: 42);
int result = star.NextInt(0, 10);
```

The `XORShift64_Star` option selects the associated xorshift transition and multiplier when constructed from a seed number. It uses the shift sizes and multiplier of the xorshift64* example in [Vigna’s paper](https://vigna.di.unimi.it/ftp/papers/xorshift.pdf), but the implementation’s middle shift goes **right**, whereas the paper’s goes **left**. It is therefore not the exact published generator. Passing a custom `ISeedR` lets you pair the output source with a different seed progression; see [seed generators](seed-generators.md).

---

← Previous: [2.2 RandomLib: random sources](random-sources.md) · [Parent: 2.2 RandomLib: random sources](random-sources.md) · Next: [2.2.2 NetRandom](random-sources-netrandom.md) →
