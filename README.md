# RandEXom

**What is RandEXom?**

RandEXom is a C# randomization library for .NET Standard 2.1. It provides interchangeable random sources and seed generators, plus helpers for shuffling, weighted picks, percentages, and number ranges.

## Quick start

Download [RandEXom.dll v1.5b](https://miputra.github.io/RandEXom/downloads/RandEXom.dll) or get it from the [GitHub release](https://github.com/miputra/RandEXom/releases/tag/1.5b), then add the DLL as a reference to a compatible .NET project.

```csharp
using RandEXom.RandomLib;

var random = new ModuloRandom(seed: 12345);
int value = random.NextInt(0, 100); // 0 through 99
```

Random range methods include the minimum and exclude the maximum. See the [usage guide](RandEXom/articles/intro.md) for examples of seeded generators, shuffling, weighted picks, and percentages.

**Version note:** v1.5b fixes generator and range calculations. Seeded sequences may differ from earlier releases.


***


## Requirements

The library targets .NET Standard 2.1.

The test and verification projects target .NET 8.


***

## Features

### Random Library
- Modulo random
- default .Net random
- System-Supplied Random Number Generators ([Based on Numerical Recipes in C The Art of Scientific Computing 2nd ed](https://www.amazon.com/Numerical-Recipes-Scientific-Computing-Second/dp/0521431085) )

### Seed Library
- Iterative
- Linear Congruential Generators ([Based on Numerical Recipes in C The Art of Scientific Computing 2nd ed](https://www.amazon.com/Numerical-Recipes-Scientific-Computing-Second/dp/0521431085) )
- default Seed
- XORShift64 (Based on research by [George Marsaglia](https://www.jstatsoft.org/article/view/v008i14))

### Framework
| Boolean | 

 - True Percentage Randomization

| Item |

- Gacha
- [Shuffles Fisher Yates](https://digital.library.adelaide.edu.au/items/8d93d3ef-a4d1-4c27-b976-5bcdf3cce5eb)

| Number |

- Distance
- Distributed Tree
- Pong
- Slot

## Download

- [RandEXom.dll v1.5b](https://miputra.github.io/RandEXom/downloads/RandEXom.dll)
- [GitHub release](https://github.com/miputra/RandEXom/releases/tag/1.5b)

The v1.5b release fixes generator and range calculations. Seeded sequences may differ from earlier releases.

## How to use

Note: Every IDE has a different way to add .dll as a reference. 

If you use the visual studio on your project:

1. In Solution Explorer, right-click your project, then click add reference
2. On the bottom left window, click the browser button, and choose RandEXom.dll
3. Now you can using RandEXom library in your project


***



## Verification

Run `dotnet run --project Verification/Verification.csproj` to check the published LCG sequence and the library's range, weighted draw, percentage, and distance edge cases. Random range methods use an inclusive minimum and exclusive maximum.

## Support the project

If RandEXom has been useful, you can support its development through [Trakteer](https://trakteer.id/miputra?quantity=1) or send crypto directly:

| Asset | Network | Address |
| --- | --- | --- |
| BNB | BNB Smart Chain (BEP-20) | `0xE122d7d44604d59b27Ca3FAA44Fc1Da94CE0aE03` |
| SOL | Solana | `C8A5J9w7UkXeLVeCWhpP47SPCPvnuS55RJWybzFhB3jF` |

Before sending, check both the network and the full address in your wallet. Crypto transfers to an incorrect address or network may not be recoverable. Donations are optional and do not affect the MIT license.
