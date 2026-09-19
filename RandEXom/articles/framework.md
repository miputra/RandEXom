# Framework: behaviors built on draws

`RandEXom.Framework` is the task layer. A framework class accepts or creates an `IRandomR` source, then adds rules around its draws: decide whether a probability succeeds, reorder or pick items, or constrain a number. It is not a replacement for `RandomLib`. If all you need is an integer in `[min, max)`, call `IRandomR.NextInt` directly; `SlotR` is a wrapper around that operation.

```text
Framework.Boolean.TruePercentageR ── compare a bounded draw to a probability
Framework.Item.Shuffles_FisherYates ── use draws to swap list positions
Framework.Item.GachaR<T> / GachaRBatched<T> ── use draws to remove items from a pool
Framework.Number.SlotR / PongR / DistanceR ── apply rules to numeric draws
                 │
                 └── each uses IRandomR ← RandomLib ← ISeedR ← SeedLib
```

## Choose a behavior

| You want | Type | Behavior and state |
| --- | --- | --- |
| A `true`/`false` event with a specified chance | `TruePercentageR` | Independent probability on each call; no quota |
| A rearranged existing list | `Shuffles_FisherYates` | Mutates the list in place |
| A weighted bag that can refill automatically | `GachaR<T>` | Stores individual copies, removes one per pull |
| A weighted bag with large counts | `GachaRBatched<T>` | Stores counts, removes one per pull, manual refill |
| A basic integer slot | `SlotR` | Delegates to its random source |
| Alternating low/high halves of a range | `PongR` | Maintains alternation state |
| Numbers separated from the previous result | `DistanceR` | Remembers the previous draw |

The `DistributedTreeR` and `DistributedTreeNestedR` number classes are obsolete/experimental; dice and noise classes are unfinished and `DEBUG`-only. See [Framework.Number](numbers.md) rather than treating them as supported alternatives.

## Default versus supplied sources

Most helpers offer a default constructor, a numeric-seed constructor, and a constructor accepting `IRandomR`. The first two create a `NetRandom` internally. The third gives you control over the source and its seed algorithm:

```csharp
using RandEXom.Framework.Number;
using RandEXom.RandomLib;

var source = new ModuloRandom(seed: 42);
var spaced = new DistanceR(source, distance: 3);
int next = spaced.Next(0, 10);
```

If two helpers receive the *same* `source` object, their draws interleave and influence each other's later results. If they each construct a source from the same numeric seed, they have separate state. Framework classes may also maintain their own state—such as a bag's remaining items or `DistanceR`'s last result—which is distinct from random-source state. A helper's `Reset()` or `Refill()` does not generally rewind its source.

For operations and examples, continue to [Boolean](boolean.md), [Item](items.md), or [Number](numbers.md). The [class index](../api/index.md) maps every type, and [how the library fits together](architecture.md) explains the layer underneath.
