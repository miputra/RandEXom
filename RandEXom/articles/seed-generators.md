# 2.1 SeedLib: seed generators

Chapter 2: Core libraries · Page 1 of 2 · [Guide map](../index.md)

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

## `XORShift64Seed`

This is `ModuloRandom`'s default seed generator. Choose the standard xorshift64 option, the `Xorshift64_star` option, or custom shift counts. A zero initial state is replaced internally with a nonzero state, while `init` still reports the supplied zero.

The xorshift family comes from George Marsaglia’s [“Xorshift RNGs” (2003)](https://www.jstatsoft.org/article/view/v008i14). The `Xorshift64_star` option matches the right, left, right shifts (12, 25, 27) in Figure 10 of Sebastiano Vigna’s [paper on scrambled xorshift generators](https://vigna.di.unimi.it/ftp/papers/xorshift.pdf). `ModuloRandom` supplies the paper’s multiplier for its `XORShift64_Star` option, scrambling output without changing the seed generator’s state. See [research and provenance](research.md). The paper tests the raw generator, not RandEXom’s bounded-number or byte APIs.

```csharp
var seed = new XORShift64Seed(XORShift64Seed.Type.Xorshift64_star, seed: 42);
var random = new ModuloRandom(seed, ModuloRandom.Multiplier.XORShift64_Star);
```

Custom shifts use `new XORShift64Seed(shift1, shift2, shift3, seed: 42)`; each shift must be from 1 through 63. Custom parameters are not automatically vetted for statistical quality.

## `LCGSeedR`

`LCGSeedR` advances a linear congruential generator: its next state depends on a multiplier `a`, increment `c`, and modulus `m`. The default parameters are the *[Numerical Recipes in C](https://numerical.recipes/)* preset; that source is a **book**, not a paper. For research on LCG parameter quality and lattice structure, see Pierre L’Ecuyer’s [1999 paper](https://www.ams.org/mcom/1999-68-225/S0025-5718-99-00996-5/S0025-5718-99-00996-5.pdf). You may choose one of the `ParameterTemplate` values or supply parameters directly:

```csharp
var seed = new LCGSeedR(seed: 42, a: 1664525, c: 1013904223, m: 4294967296);
seed.Next();
long current = seed.now;
```

`m` must be greater than one. The available preset names are the same as those listed for `SSRNGRandom`; they are historical parameter sets and should not be treated as cryptographic options.

## `IterativeSeedR`

`IterativeSeedR` advances by a selected process. It starts with `Addition`; other options are `Substraction` (the API's spelling), `Multiplication`, `Cos`, and `Sin`.

```csharp
var seed = new IterativeSeedR(seed: 42);
seed.process = IterativeSeedR.Process.Multiplication;
var random = new ModuloRandom(seed);
```

The process field is mutable, so changing it between draws changes the sequence. Arithmetic on `long` values may wrap at the limits of the type; do not assume these options have good statistical properties.

## `IterativeSeedRCustom`

Provide a function that maps the current state to the next one:

```csharp
var seed = new IterativeSeedRCustom(current => unchecked(current * 17 + 3), seed: 42);
var random = new ModuloRandom(seed);
```

The function cannot be `null`. Avoid functions that immediately converge to a constant or cycle over a tiny set if you need varied output.

## Constant seed (`SeedR`)

The library also contains `SeedR`, whose `Next()` does not change its value. It is **internal**, so consumer projects cannot instantiate it directly. Default constructors of helpers and `NetRandom` use it internally. If you need a fixed seed with `NetRandom`, use `new NetRandom(42)`.

---

← Previous: [1.3 Getting started](intro.md) · [Guide map](../index.md) · Next: [2.2 RandomLib](random-sources.md) →
