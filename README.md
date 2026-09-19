# RandEXom

RandEXom is a C# randomization library for .NET Standard 2.1. It provides interchangeable random sources and seed generators, plus helpers for shuffling, weighted picks, percentages, and number ranges.

## Quick start

Download [RandEXom.dll v1.5b](https://miputra.github.io/RandEXom/downloads/RandEXom.dll) or get it from the [GitHub release](https://github.com/miputra/RandEXom/releases/tag/1.5b), then add the DLL as a reference to a compatible .NET project.

```csharp
using RandEXom.RandomLib;

var random = new ModuloRandom(seed: 12345);
int value = random.NextInt(0, 100); // 0 through 99
```

Random range methods include the minimum and exclude the maximum.

**Version note:** v1.5b fixes generator and range calculations. Seeded sequences may differ from earlier releases.

## Documentation

| Guide | What it covers |
| --- | --- |
| [Getting started](RandEXom/articles/intro.md) | First program and common examples |
| [Installation](RandEXom/articles/installation.md) | DLL and source-project setup, requirements, verification |
| [Random sources](RandEXom/articles/random-sources.md) | `ModuloRandom`, `NetRandom`, `SSRNGRandom`, range and byte APIs |
| [Seed generators](RandEXom/articles/seed-generators.md) | Xorshift, LCG, iterative, custom, and constant seeds |
| [Boolean helpers](RandEXom/articles/boolean.md) | `TruePercentageR` |
| [Item helpers](RandEXom/articles/items.md) | Fisher–Yates shuffle and both gacha implementations |
| [Number helpers](RandEXom/articles/numbers.md) | Slot, pong, distance, and experimental types |
| [Custom implementations and limitations](RandEXom/articles/extending.md) | Interfaces, shared state, reproducibility, safety |

The [documentation home](RandEXom/index.md) and [API overview](RandEXom/api/index.md) are also included in the repository. The library targets .NET Standard 2.1; the verification project targets .NET 8. There is no published NuGet package listed here—use the release DLL or a source-project reference.

## Verification

Run `dotnet run --project Verification/Verification.csproj` to check the published LCG sequence and the library's range, weighted draw, percentage, and distance edge cases.

## Support the project

If RandEXom has been useful, you can support its development through [Trakteer](https://trakteer.id/miputra?quantity=1) or send crypto directly:

| Asset | Network | Address |
| --- | --- | --- |
| BNB | BNB Smart Chain (BEP-20) | `0xE122d7d44604d59b27Ca3FAA44Fc1Da94CE0aE03` |
| SOL | Solana | `C8A5J9w7UkXeLVeCWhpP47SPCPvnuS55RJWybzFhB3jF` |

Before sending, check both the network and the full address in your wallet. Crypto transfers to an incorrect address or network may not be recoverable. Donations are optional and do not affect the MIT license.
