# How RandEXom fits together

RandEXom is arranged in layers. A **seed generator** supplies state, a **random source** turns that state into bounded numbers or bytes, and a **framework helper** applies those draws to a task such as shuffling or drawing from a bag. The interfaces connect the layers; utilities support internal conversions and optional time-based seed creation.

```text
Your application
  └─ Framework.Boolean / Framework.Item / Framework.Number  (optional task helpers)
       └─ IRandomR ← RandomLib                              (draw numbers and bytes)
            └─ ISeedR ← SeedLib                              (hold/advance seed state)

Utility ── supporting functions; not a required layer in application code
```

You can use `RandomLib` directly without a framework helper. Most helpers also have constructors that build their own default `NetRandom`; supplying an `IRandomR` yourself lets you choose and share the source. No framework helper is necessary merely to get a bounded number.

## Namespaces and responsibilities

| Namespace | Question it answers | Main contract or types | Read next |
| --- | --- | --- | --- |
| `RandEXom.Interface` | What can be plugged into another layer? | `ISeedR`, `IRandomR`, `IGachaR<T>` | [Contracts and extension](extending.md) |
| `RandEXom.SeedLib` | How does the seed state change? | `XORShift64Seed`, `LCGSeedR`, `IterativeSeedR`, `IterativeSeedRCustom` | [SeedLib](seed-generators.md) |
| `RandEXom.RandomLib` | How are bounded values and bytes generated? | `ModuloRandom`, `NetRandom`, `SSRNGRandom` | [RandomLib](random-sources.md) |
| `RandEXom.Framework` | Which task-level behavior wraps a source? | Boolean, Item, and Number branches | [Framework overview](framework.md) |
| `RandEXom.Framework.Boolean` | Should this independent chance succeed? | `TruePercentageR` | [Boolean](boolean.md) |
| `RandEXom.Framework.Item` | How do I reorder or draw items? | `Shuffles_FisherYates`, `GachaR<T>`, `GachaRBatched<T>` | [Item](items.md) |
| `RandEXom.Framework.Number` | What range behavior do I need? | `SlotR`, `PongR`, `DistanceR`; experimental tree types | [Number](numbers.md) |
| `RandEXom.Utility` | What supporting conversions exist? | public `SeedGenerator`; internal `TypeR` | [Utility](utility.md) |

The [class index](../api/index.md) maps each type to its guide and availability. `Framework` means an application-level behavior built *on top of* a random source, not a separate random engine.

## Follow one draw through the layers

```csharp
using RandEXom.Framework.Boolean;
using RandEXom.RandomLib;
using RandEXom.SeedLib;

var seed = new LCGSeedR(LCGSeedR.ParameterTemplate.Numerical_Recipes, seed: 42);
var random = new ModuloRandom(seed);
var chance = new TruePercentageR(random, percentage: 25f);
bool result = chance.Next();
```

1. `LCGSeedR` starts from seed 42 and implements `ISeedR`.
2. `ModuloRandom` holds that seed generator and implements `IRandomR`.
3. `TruePercentageR` asks its random source for a bounded value and compares it with a threshold representing 25%.
4. The draw advances state. The next `chance.Next()` uses the subsequent state, not a fresh copy of seed 42.

Changing *either* the seed algorithm or the random-source algorithm changes the resulting sequence. A seed is an input to an algorithm, not a complete specification of the output. Some helpers construct `NetRandom` with a constant internal seed by default; passing a `RandomLib` implementation explicitly makes the composition visible.

## How to choose a path

| Goal | Start here | Why |
| --- | --- | --- |
| Draw an integer or fill bytes | `RandomLib.ModuloRandom` | Direct `NextInt`, `NextLong`, `NextBytes` API |
| Reproduce a particular seed progression | A `SeedLib` implementation, then a `RandomLib` source | Seed and source are separate choices |
| Shuffle or draw without replacement | `Framework.Item` | Adds list or bag behavior to a source |
| Draw a probability or constrained number | `Framework.Boolean` or `.Number` | Adds rules to a source; see [Framework](framework.md) |
| Supply a different algorithm | `ISeedR` or `IRandomR` | Plug into existing constructors |
| Generate a date-based numeric seed | `Utility.SeedGenerator` | Optional convenience; not security-grade |

All bounded range examples use `[min, max)`: the lower bound is included, the upper bound is excluded. The generators here are not cryptographic random-number generators. See [limitations](extending.md) before relying on sequences across versions or runtimes.
