# Random sources

Random sources implement `RandEXom.Interface.IRandomR`. All expose `NextInt(min, max)`, `NextLong(min, max)`, `NextBytes(buffer)`, `GetSeed()`, and `GetSeedSTR()`. For the range methods, `min` is included and `max` is excluded. Always pass `min < max`; behavior for an empty range differs between implementations. The methods advance internal state, so repeated calls usually produce different results.

```csharp
using RandEXom.RandomLib;

var random = new ModuloRandom(seed: 12345);
int die = random.NextInt(1, 7);
long sample = random.NextLong(-1000, 1000);
byte[] bytes = new byte[16];
random.NextBytes(bytes);
```

The byte array is filled in place. `GetSeedSTR()` returns the initial seed as text, not a serialized snapshot of the generator's current state. A fixed seed can reproduce a sequence with the same generator and library version, but v1.5b changed some sequences from earlier releases.

## `ModuloRandom`

`ModuloRandom` is the simple general-purpose choice in this library. By default it advances an `XORShift64Seed`; you can pass a seed number, an `ISeedR`, or a multiplier option. Its bounded methods use rejection sampling to avoid modulo bias.

```csharp
using RandEXom.RandomLib;

var regular = new ModuloRandom(seed: 42);
var star = new ModuloRandom(ModuloRandom.Multiplier.XORShift64_Star, seed: 42);
int result = star.NextInt(0, 10);
```

The `XORShift64_Star` option selects the matching xorshift transition and multiplier when constructed from a seed number. Passing a custom `ISeedR` lets you pair the output source with a different seed progression; see [seed generators](seed-generators.md).

## `NetRandom`

`NetRandom` wraps `System.Random`. It accepts an optional numeric seed or an `ISeedR`. Its `NextLong` uses random bytes to sample the requested range.

```csharp
using RandEXom.RandomLib;

var random = new NetRandom(seed: 42);
int index = random.NextInt(0, 5);
```

`NetRandom` converts its seed to a 32-bit value for `System.Random`; different 64-bit seeds can therefore map to the same internal seed. For reproducibility, also keep the same target runtime: `System.Random` is a .NET implementation detail, not a cross-runtime sequence contract.

## `SSRNGRandom`

`SSRNGRandom` exposes historical system-supplied LCG parameter presets. It can construct a matching `LCGSeedR` from a preset, or accept an `ISeedR` and an output modulus. Use it when a particular generator family or preset is part of your experiment, not as a security primitive.

```csharp
using RandEXom.RandomLib;

var random = new SSRNGRandom(
    SSRNGRandom.ParameterTemplate.Numerical_Recipes,
    seed: 42);
int value = random.NextInt(0, 100);
```

Available presets: `ZX81`, `Numerical_Recipes`, `Borland_C`, `GLIBC`, `IBM`, `Borland_Delphi`, `Turbo_Pascal`, `Microsoft_Visual_C`, `Microsoft_Visual_Basic`, `RtlUniform`, `Apple_CarbonLib`, `C_Plus_11`, `MTH_RANDOM`, `Java`, `random0`, `POSIX`, `cc65`, `cc65_2`, and `RANDU`. The output modulus must be greater than one.

## Choosing a source

| Need | Starting point |
| --- | --- |
| A seeded random source for application or simulation use | `ModuloRandom(seed: ...)` |
| The platform `System.Random` implementation | `NetRandom(seed: ...)` |
| A historical LCG preset | `SSRNGRandom(ParameterTemplate, seed: ...)` |

None of these classes is documented as cryptographically secure. Use a platform cryptographic random-number generator for tokens, passwords, keys, or other security-sensitive values.
