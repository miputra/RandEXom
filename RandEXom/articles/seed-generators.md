# Seed generators

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

This is `ModuloRandom`'s default seed generator. Choose standard xorshift64, xorshift64* transition parameters, or custom shift counts. A zero initial state is replaced internally with a nonzero state, while `init` still reports the supplied zero.

```csharp
var seed = new XORShift64Seed(XORShift64Seed.Type.Xorshift64_star, seed: 42);
var random = new ModuloRandom(seed, ModuloRandom.Multiplier.XORShift64_Star);
```

Custom shifts use `new XORShift64Seed(shift1, shift2, shift3, seed: 42)`; each shift must be from 1 through 63. Custom parameters are not automatically vetted for statistical quality.

## `LCGSeedR`

`LCGSeedR` advances a linear congruential generator: its next state depends on a multiplier `a`, increment `c`, and modulus `m`. The default parameters are the Numerical Recipes preset. You may choose one of its `ParameterTemplate` values or supply parameters directly:

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
