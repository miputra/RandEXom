# 2.1.3 `IterativeSeedR`

[Parent: 2.1 SeedLib: seed generators](seed-generators.md) · [Guide map](../index.md)

`IterativeSeedR` advances by a selected process. It starts with `Addition`; other options are `Substraction` (the API's spelling), `Multiplication`, `Cos`, and `Sin`.

```csharp
using RandEXom.SeedLib;
using RandEXom.RandomLib;

var seed = new IterativeSeedR(seed: 42);
seed.process = IterativeSeedR.Process.Multiplication;
var random = new ModuloRandom(seed);
```

The process field is mutable, so changing it between draws changes the sequence. Arithmetic on `long` values may wrap at the limits of the type; do not assume these options have good statistical properties.

---

← Previous: [2.1.2 LCGSeedR](seed-generators-lcgseedr.md) · [Parent: 2.1 SeedLib: seed generators](seed-generators.md) · Next: [2.1.4 IterativeSeedRCustom](seed-generators-iterativeseedrcustom.md) →
