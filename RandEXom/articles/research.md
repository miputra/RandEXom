# 4.3 Research and provenance

Chapter 4: Reference · Page 3 of 4 · [Guide map](../index.md)

This page distinguishes **implemented algorithms**, **parameter sources**, and **research related to an unfinished placeholder**. A citation is not a claim that RandEXom reproduces every method or quality result in that work. Follow the feature guides for what this code actually does.

## Implemented algorithm families

| RandEXom feature | Research paper | What the link supports |
| --- | --- | --- |
| `XORShift64Seed` (`Xorshift64`) | George Marsaglia, [“Xorshift RNGs” (2003)](https://www.jstatsoft.org/article/view/v008i14) | The xorshift generator family; RandEXom uses a 64-bit shift/xor state transition. |
| `XORShift64Seed` and `ModuloRandom` (`XORShift64_Star`) | Sebastiano Vigna, [“An experimental exploration of Marsaglia’s xorshift generators, scrambled” (2016)](https://vigna.di.unimi.it/ftp/papers/xorshift.pdf) | The built-in star option matches Figure 10’s right–left–right transition (12, 25, 27) and output multiplier (2685821657736338717). The seed generator retains the unmultiplied state. The paper’s statistical results concern the raw generator, not RandEXom’s bounded-number or byte APIs, nor arbitrary custom seed generators paired with the multiplier. |
| `Shuffles_FisherYates` | Richard Durstenfeld, [“Algorithm 235: Random permutation” (1964)](https://doi.org/10.1145/364520.364540) | The in-place swap approach to random permutation. RandEXom uses a forward traversal equivalent to the paper’s backward traversal. |

The name “Fisher–Yates” also acknowledges the earlier shuffle procedure by Ronald Fisher and Frank Yates. Their *Statistical Tables for Biological, Agricultural and Medical Research* is a **book**, not the linked Durstenfeld paper. The [1938 library catalog record](https://wellcomecollection.org/works/udsvs2rx) is provided for historical provenance.

## LCG research versus preset provenance

`LCGSeedR` and `SSRNGRandom` use linear-congruential recurrences. Pierre L’Ecuyer’s paper [“Tables of Linear Congruential Generators of Different Sizes and Good Lattice Structure” (1999)](https://www.ams.org/mcom/1999-68-225/S0025-5718-99-00996-5/S0025-5718-99-00996-5.pdf) discusses parameter quality and the lattice structure of LCGs. **The paper is background research, not the source of RandEXom’s named historical presets.**

The default `a = 1664525`, `c = 1013904223`, `m = 2^32` combination is from *[Numerical Recipes in C, 2nd edition, section 7.1](https://numerical.recipes/)*. That is a **book**, not a research paper. Other enum presets carry names of historical systems; their names alone do not certify the quality or exact runtime output of those systems. See [SeedLib](seed-generators.md) and [RandomLib](random-sources.md) for this implementation’s behavior.

## Related research, not an implemented feature

`PerlinNoise` is currently an empty `DEBUG`-only placeholder. Ken Perlin’s [“Improving Noise” (2002)](https://mrl.cs.nyu.edu/~perlin/paper445.pdf) is the relevant research reference for the name and a possible future implementation, **not** evidence that RandEXom currently implements Perlin noise.

## Platform behavior and project-specific helpers

`NetRandom` wraps `System.Random`; its behavior should be read from [Microsoft’s API documentation](https://learn.microsoft.com/dotnet/api/system.random), not attributed to a *Numerical Recipes* algorithm. `TruePercentageR`, `GachaR<T>`, `GachaRBatched<T>`, `SlotR`, `PongR`, and `DistanceR` are documented from the code’s behavior rather than presented as implementations of a particular paper. If research is added to their design later, it should be linked next to the exact claim it supports.

---

← Previous: [4.2 Interfaces and limitations](extending.md) · [Guide map](../index.md) · Next: [4.4 Class index](../api/index.md) →
