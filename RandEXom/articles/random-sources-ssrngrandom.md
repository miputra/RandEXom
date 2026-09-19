# 2.2.3 `SSRNGRandom`

[Parent: 2.2 RandomLib: random sources](random-sources.md) · [Guide map](../index.md)

`SSRNGRandom` exposes historical system-supplied LCG parameter presets. It can construct a matching `LCGSeedR` from a preset, or accept an `ISeedR` and an output modulus. Use it when a particular generator family or preset is part of your experiment, not as a security primitive.

```csharp
using RandEXom.RandomLib;

var random = new SSRNGRandom(
    SSRNGRandom.ParameterTemplate.Numerical_Recipes,
    seed: 42);
int value = random.NextInt(0, 100);
```

Available presets: `ZX81`, `Numerical_Recipes`, `Borland_C`, `GLIBC`, `IBM`, `Borland_Delphi`, `Turbo_Pascal`, `Microsoft_Visual_C`, `Microsoft_Visual_Basic`, `RtlUniform`, `Apple_CarbonLib`, `C_Plus_11`, `MTH_RANDOM`, `Java`, `random0`, `POSIX`, `cc65`, `cc65_2`, and `RANDU`. The output modulus must be greater than one.

The default LCG constants are attributed to the *[Numerical Recipes in C](https://numerical.recipes/)* book. [L’Ecuyer’s paper on LCG parameter quality](https://www.ams.org/mcom/1999-68-225/S0025-5718-99-00996-5/S0025-5718-99-00996-5.pdf) is relevant background, **not** the source of every named preset. See [research and provenance](research.md).

---

← Previous: [2.2.2 NetRandom](random-sources-netrandom.md) · [Parent: 2.2 RandomLib: random sources](random-sources.md) · Next: [2.2.4 Choosing a source](random-sources-choosing-a-source.md) →
