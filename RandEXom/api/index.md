
# 4.4 Class index

Chapter 4: Reference · Section 4 · [Guide map](../index.md)

This is a map from source namespaces and class names to the explanatory guide. Start with [the architecture](../articles/architecture.md) if `SeedLib`, `RandomLib`, and `Framework` are new to you; follow [installation](../articles/installation.md) and [getting started](../articles/intro.md) for a first program. The [source tree](https://github.com/miputra/RandEXom/tree/main/RandEXom) has exact signatures and XML comments.

<a id="contracts-randexom-interface"></a>

## 4.4.1 Contracts (`RandEXom.Interface`)

| Type | Connects | Guide |
| --- | --- | --- |
| `ISeedR` | A state generator to a random source | [SeedLib](../articles/seed-generators.md), [implementing interfaces](../articles/extending.md#implement-an-interface) |
| `IRandomR` | A random source to framework helpers | [RandomLib](../articles/random-sources.md), [implementing interfaces](../articles/extending.md#implement-an-interface) |
| `IGachaR<T>` | Shared bag operations for both item-pool variants | [Framework.Item](../articles/items.md) |

<a id="state-randexom-seedlib"></a>

## 4.4.2 State (`RandEXom.SeedLib`)

| Type | Role | Guide |
| --- | --- | --- |
| `XORShift64Seed` | Xorshift state, default for `ModuloRandom` | [Open subchapter](../articles/seed-generators-xorshift64seed.md) |
| `LCGSeedR` | Linear-congruential state and presets | [Open subchapter](../articles/seed-generators-lcgseedr.md) |
| `IterativeSeedR` | Built-in arithmetic/trigonometric progressions | [Open subchapter](../articles/seed-generators-iterativeseedr.md) |
| `IterativeSeedRCustom` | Function-defined progression | [Open subchapter](../articles/seed-generators-iterativeseedrcustom.md) |
| `SeedR` | Constant internal state; not public to consumers | [Open subchapter](../articles/seed-generators-seedr.md) |

<a id="draws-randexom-randomlib"></a>

## 4.4.3 Draws (`RandEXom.RandomLib`)

| Type | Role | Guide |
| --- | --- | --- |
| `ModuloRandom` | Xorshift-backed bounded source by default | [Open subchapter](../articles/random-sources-modulorandom.md) |
| `NetRandom` | Adapter around `System.Random` | [Open subchapter](../articles/random-sources-netrandom.md) |
| `SSRNGRandom` | Historical LCG parameter-based source | [Open subchapter](../articles/random-sources-ssrngrandom.md) |

<a id="behaviors-randexom-framework"></a>

## 4.4.4 Behaviors (`RandEXom.Framework`)

| Namespace and type | Role | Guide |
| --- | --- | --- |
| `Boolean.TruePercentageR` | Independent percentage outcome | [Open subchapter](../articles/boolean-truepercentager.md) |
| `Item.Shuffles_FisherYates` | In-place list shuffle | [Open subchapter](../articles/items-shuffles-fisheryates.md) |
| `Item.GachaR<T>` | List-backed weighted bag with optional automatic refill | [Open subchapter](../articles/items-gachar.md) |
| `Item.GachaRBatched<T>` | Count-backed weighted bag with manual refill | [Open subchapter](../articles/items-gacharbatched.md) |
| `Number.SlotR` | One bounded draw per call, intended for spinning reels one by one | [Open subchapter](../articles/numbers-slotr.md) |
| `Number.PongR` | Alternating lower/upper range halves | [Open subchapter](../articles/numbers-pongr.md) |
| `Number.DistanceR` | Minimum gap from the previous result | [Open subchapter](../articles/numbers-distancer.md) |
| `Item.DistributedTreeR<T>` | Random branch item picker with optional removal and empty-endpoint locking | [Open subchapter](../articles/items-distributedtreer.md) |
| `_2DNoise.PerlinNoise` | Empty, `DEBUG`-only placeholder | [Research context](../articles/research.md#related-research-not-an-implemented-feature) |

<a id="support-randexom-utility"></a>

## 4.4.5 Support (`RandEXom.Utility`)

| Type | Availability and role | Guide |
| --- | --- | --- |
| `SeedGenerator` | Public local clock and Random.org seed helpers | [Open subchapter](../articles/utility-seedgenerator.md) |
| `TypeR` | Internal 64-to-32-bit seed conversion used by `NetRandom` | [Open subchapter](../articles/utility-typer.md) |

This is an authored index, not generated member-by-member API metadata. Its purpose is to make the layer boundaries and type names discoverable; for overload signatures, see the linked source and feature guides.

---

← Previous: [4.3 Research and provenance](../articles/research.md) · [Guide map](../index.md) · Next: [4.5 TestOut](../articles/test-output.md) →
