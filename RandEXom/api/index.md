
# API reference

The public types are grouped by namespace:

- `RandEXom.RandomLib`: bounded random sources (`ModuloRandom`, `NetRandom`, `SSRNGRandom`).
- `RandEXom.SeedLib`: seed generators, including `LCGSeedR` and `XORShift64Seed`.
- `RandEXom.Framework.Boolean`, `.Item`, and `.Number`: percentage, shuffle, weighted-draw, and range helpers.
- `RandEXom.Interface`: contracts for custom random and seed implementations.

Use the [getting-started guide](../articles/intro.md) for examples. The [source tree](https://github.com/miputra/RandEXom/tree/main/RandEXom) contains the current method signatures and XML comments. When building the DocFX site, the project file is included by `docfx.json` to generate type pages under this section.
