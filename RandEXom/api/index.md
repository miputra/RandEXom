
# Class index

This is a map from source namespaces and class names to the explanatory guide. Start with [the architecture](../articles/architecture.md) if `SeedLib`, `RandomLib`, and `Framework` are new to you; follow [installation](../articles/installation.md) and [getting started](../articles/intro.md) for a first program. The [source tree](https://github.com/miputra/RandEXom/tree/main/RandEXom) has exact signatures and XML comments.

## Contracts (`RandEXom.Interface`)

| Type | Connects | Guide |
| --- | --- | --- |
| `ISeedR` | A state generator to a random source | [SeedLib](../articles/seed-generators.md), [implementing interfaces](../articles/extending.md) |
| `IRandomR` | A random source to framework helpers | [RandomLib](../articles/random-sources.md), [implementing interfaces](../articles/extending.md) |
| `IGachaR<T>` | Shared bag operations for both item-pool variants | [Framework.Item](../articles/items.md) |

## State (`RandEXom.SeedLib`)

| Type | Role | Guide |
| --- | --- | --- |
| `XORShift64Seed` | Xorshift state, default for `ModuloRandom` | [SeedLib](../articles/seed-generators.md) |
| `LCGSeedR` | Linear-congruential state and presets | [SeedLib](../articles/seed-generators.md) |
| `IterativeSeedR` | Built-in arithmetic/trigonometric progressions | [SeedLib](../articles/seed-generators.md) |
| `IterativeSeedRCustom` | Function-defined progression | [SeedLib](../articles/seed-generators.md) |
| `SeedR` | Constant internal state; not public to consumers | [SeedLib](../articles/seed-generators.md) |

## Draws (`RandEXom.RandomLib`)

| Type | Role | Guide |
| --- | --- | --- |
| `ModuloRandom` | Xorshift-backed bounded source by default | [RandomLib](../articles/random-sources.md) |
| `NetRandom` | Adapter around `System.Random` | [RandomLib](../articles/random-sources.md) |
| `SSRNGRandom` | Historical LCG parameter-based source | [RandomLib](../articles/random-sources.md) |

## Behaviors (`RandEXom.Framework`)

| Namespace and type | Role | Guide |
| --- | --- | --- |
| `Boolean.TruePercentageR` | Independent percentage outcome | [Framework.Boolean](../articles/boolean.md) |
| `Item.Shuffles_FisherYates` | In-place list shuffle | [Framework.Item](../articles/items.md) |
| `Item.GachaR<T>` | List-backed weighted bag with optional automatic refill | [Framework.Item](../articles/items.md) |
| `Item.GachaRBatched<T>` | Count-backed weighted bag with manual refill | [Framework.Item](../articles/items.md) |
| `Number.SlotR` | One integer within a range | [Framework.Number](../articles/numbers.md) |
| `Number.PongR` | Alternating lower/upper range halves | [Framework.Number](../articles/numbers.md) |
| `Number.DistanceR` | Minimum gap from the previous result | [Framework.Number](../articles/numbers.md) |
| `Number.DistributedTreeR` | Obsolete/experimental tree distribution | [Framework.Number](../articles/numbers.md) |
| `Number.DistributedTreeNestedR` | Obsolete/experimental nested tree | [Framework.Number](../articles/numbers.md) |
| `Number.DiceR`, `Number.Dice6F`, `_2DNoise.PerlinNoise` | Incomplete, `DEBUG`-only types | [Framework.Number](../articles/numbers.md) |

## Support (`RandEXom.Utility`)

| Type | Availability and role | Guide |
| --- | --- | --- |
| `SeedGenerator` | Public time-derived numeric seed helper | [Utility](../articles/utility.md) |
| `TypeR` | Internal 64-to-32-bit seed conversion used by `NetRandom` | [Utility](../articles/utility.md) |

This is an authored index, not generated member-by-member API metadata. Its purpose is to make the layer boundaries and type names discoverable; for overload signatures, see the linked source and feature guides.
