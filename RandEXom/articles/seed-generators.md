# 2.1 SeedLib: seed generators

Chapter 2: Core libraries · Section 1 · [Guide map](../index.md)

`SeedLib` is the **state layer**. It does not offer a `NextInt(min, max)` method or shuffle a list. A seed generator implements `RandEXom.Interface.ISeedR` and exposes `init` (original input), `now` (current state), `previous` (state before the latest advance), and `Next()` to advance. A `RandomLib` source consumes that state and provides bounded draws; a `Framework` helper can then use those draws. See the [layer diagram](architecture.md) and [class index](../api/index.md).

| Choose | When you need |
| --- | --- |
| `XORShift64Seed` | The default progression of `ModuloRandom` or explicit xorshift variants |
| `LCGSeedR` | A parameterized LCG or historical preset |
| `IterativeSeedR` | A simple built-in update operation |
| `IterativeSeedRCustom` | Your own state-update function |
| `SeedR` | Constant state inside library defaults; not publicly constructible |

The source decides *when* to call `Next()`. For example, `ModuloRandom` advances the seed before each raw draw, whereas `SSRNGRandom` reads a state digit and then advances. Therefore, swapping the source while retaining the same `ISeedR` is not sequence-preserving.

```csharp
using RandEXom.RandomLib;
using RandEXom.SeedLib;

var seed = new LCGSeedR(LCGSeedR.ParameterTemplate.Numerical_Recipes, seed: 42);
var random = new ModuloRandom(seed);
int value = random.NextInt(0, 10);
```

If you omit a seed, each class chooses one from the current time. Provide an explicit seed when you need a reproducible sequence. The same initial seed is not enough to make different algorithms produce identical output.

## In this chapter

- <a id="xorshift64seed"></a>[2.1.1 XORShift64Seed](seed-generators-xorshift64seed.md)
- <a id="lcgseedr"></a>[2.1.2 LCGSeedR](seed-generators-lcgseedr.md)
- <a id="iterativeseedr"></a>[2.1.3 IterativeSeedR](seed-generators-iterativeseedr.md)
- <a id="iterativeseedrcustom"></a>[2.1.4 IterativeSeedRCustom](seed-generators-iterativeseedrcustom.md)
- <a id="constant-seed-seedr"></a>[2.1.5 Constant seed (SeedR)](seed-generators-seedr.md)

---

← Previous: [1.3 Getting started](intro.md) · [Guide map](../index.md) · Next: [2.1.1 XORShift64Seed](seed-generators-xorshift64seed.md) →