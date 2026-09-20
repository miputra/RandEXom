
# 1.3 Getting started

Chapter 1: Start here · Section 3 · [Guide map](../index.md)

First read [how the library fits together](architecture.md): `SeedLib` holds state, `RandomLib` produces values, and `Framework` applies values to tasks. This page then gives a quick path from download to a few common draws. For a type-by-type lookup, use the [class index](../api/index.md).

RandEXom targets .NET Standard 2.1. Download the DLL from the [latest source build release](https://github.com/miputra/RandEXom/releases/tag/build-2026-09-20), then add it as a reference to a compatible .NET project. In Visual Studio, right-click the project in Solution Explorer, choose **Add Reference**, browse to the DLL, and confirm.

<a id="generate-a-bounded-number"></a>

## 1.3.1 Generate a bounded number

```csharp
using RandEXom.RandomLib;

var random = new ModuloRandom(seed: 12345);
int roll = random.NextInt(1, 7); // 1 through 6
long sample = random.NextLong(0, 1000); // 0 through 999
```

The minimum is inclusive and the maximum is exclusive. The minimum must be smaller than the maximum. Supplying a seed makes the generator's sequence reproducible for the same library version; v1.5b corrected generator behavior, so do not assume its output matches older releases. These generators are for simulation and application randomization, not cryptographic secrets.

<a id="choose-a-seed-generator"></a>

## 1.3.2 Choose a seed generator

`ModuloRandom` accepts an `ISeedR` implementation. For example, use the Numerical Recipes LCG parameters with a known seed:

```csharp
using RandEXom.RandomLib;
using RandEXom.SeedLib;

var seed = new LCGSeedR(LCGSeedR.ParameterTemplate.Numerical_Recipes, seed: 42);
var random = new ModuloRandom(seed);
int value = random.NextInt(0, 10);
```

<a id="shuffle-and-draw-items"></a>

## 1.3.3 Shuffle and draw items

```csharp
using System.Collections.Generic;
using RandEXom.Framework.Item;

var cards = new List<string> { "red", "blue", "green" };
new Shuffles_FisherYates(seed: 42).Shuffle(cards); // modifies the list in place

var bag = new GachaR<string>(seed: 42, reset_on_empty: false);
bag.AddItem("common", 3);
bag.AddItem("rare", 1);
string drawn = bag.Pull(); // removes one item from the bag
```

`AddItem` adds `count` copies, so adding more copies gives an item more chances to be drawn. With `reset_on_empty: false`, `Pull()` returns the type's default value when the bag is empty; the default behavior refills it instead. Use `Refill()` to restore the initial contents manually.

<a id="draw-a-percentage"></a>

## 1.3.4 Draw a percentage

```csharp
using RandEXom.Framework.Boolean;

var succeeds = new TruePercentageR(seed: 42, percentage: 25f);
bool result = succeeds.Next(); // independent 25% chance of true
```

Percentages must be between 0 and 100. Each call is an independent draw, not a promise that exactly 25 out of 100 calls return true.

For the full public surface, see the [API overview](../api/index.md). From the repository root, run `dotnet run --project Verification/Verification.csproj` for the library's verification checks.

---

← Previous: [1.2 Installation](installation.md) · [Guide map](../index.md) · Next: [2.1 SeedLib](seed-generators.md) →
