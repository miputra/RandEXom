# 3.4 Framework.Number

Chapter 3: Framework helpers · Page 4 of 4 · [Guide map](../index.md)

This branch of [Framework](framework.md) adds different rules to numeric draws. Its usable types are `SlotR` (one draw per slot-machine reel), `PongR` (alternating halves), and `DistanceR` (gap from the last result). They live in `RandEXom.Framework.Number` and accept either a seed number or an `IRandomR` source. Their range methods include the minimum and exclude the maximum. Experimental tree and unfinished dice/noise types are listed below so their presence in source code is not mistaken for a supported feature.

## `SlotR`: spin one reel at a time

`SlotR` is inspired by a classical slot machine: call `Next(min, max)` once for **each reel or slot position**, in order. Each call draws one bounded value from the configured random source and advances that source. It does not generate an entire three-reel result in one call, remember how many reels exist, or enforce slot-machine payout rules. Your application chooses the reel count and combines the returned values.

```csharp
using RandEXom.Framework.Number;

var slot = new SlotR(seed: 42);
int[] reels = new int[3];
for (int i = 0; i < reels.Length; i++)
{
    reels[i] = slot.Next(0, 10); // draw this reel: 0 through 9
}
```

The example produces three reel values by making three sequential calls. If reels use different symbols or ranges, pass the appropriate bounds on each call. `SlotR` itself simply delegates each draw to `IRandomR.NextInt`.

## `PongR`: alternate halves

`PongR` alternates between the lower and upper halves of a range on successive calls. Basically it is like a pong ball where bounce high and low. The first call samples the lower half. The integer and long overloads maintain **separate** alternation state.

```csharp
var pong = new PongR(seed: 42);
int lower = pong.Next(0, 10); // 0 through 4
int upper = pong.Next(0, 10); // 5 through 9
```

For odd-sized ranges, the upper half contains one extra value. For a one-value range, every call returns that value without changing the alternation state. Changing ranges between calls does not reset the alternating half.

## `DistanceR`: keep draws apart

`DistanceR` samples a value at least `distance` away from its **previous** result. The first draw is unrestricted within the range.

```csharp
var spaced = new DistanceR(seed: 42, distance: 3);
int first = spaced.Next(0, 10);
int second = spaced.Next(0, 10); // |second - first| >= 3
```

`distance` must be nonnegative, and `SetDistance()` changes it for future calls. A distance of zero imposes no spacing. If no value in `[min, max)` can satisfy the requested gap, `Next()` throws `InvalidOperationException`. This is not a constraint against *all* earlier values—only the immediately preceding one. `Next` has both `int` and `long` overloads.

## Experimental distributed trees

`DistributedTreeR` and `DistributedTreeNestedR` partition a numeric range into tree leaves and favor branches with lower accumulated weights. Both expose `Next()` and `NextInt()` plus `CreateNodes(level, child)`. These types are marked obsolete. `DistributedTreeR` is also hidden from IntelliSense and has shared static node storage; `DistributedTreeNestedR` is explicitly still in development. **Do not rely on either for production behavior**. They are documented here for completeness, not recommended as an alternative to `SlotR` or `DistanceR`.

## Debug-only placeholders

`DiceR`, `Dice6F`, and `PerlinNoise` are compiled only under `DEBUG`. `DiceR` is abstract, and these classes have no usable finished randomization or noise implementation. They are not part of the documented release workflow. The Perlin name refers to Ken Perlin’s [“Improving Noise” (2002)](https://mrl.cs.nyu.edu/~perlin/paper445.pdf), but this project **does not yet implement** that paper’s algorithm.

---

← Previous: [3.3 Item](items.md) · [Guide map](../index.md) · Next: [4.1 Utility](utility.md) →
