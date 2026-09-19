# Number helpers

This branch of [Framework](framework.md) adds different rules to numeric draws. Its usable types are `SlotR` (plain range), `PongR` (alternating halves), and `DistanceR` (gap from the last result). They live in `RandEXom.Framework.Number` and accept either a seed number or an `IRandomR` source. Their range methods include the minimum and exclude the maximum. Experimental tree and unfinished dice/noise types are listed below so their presence in source code is not mistaken for a supported feature.

## `SlotR`: one bounded integer

`SlotR.Next(min, max)` delegates to the configured random source. It provides a small framework wrapper when you want an integer slot within a range.

```csharp
using RandEXom.Framework.Number;

var slot = new SlotR(seed: 42);
int digit = slot.Next(0, 10); // 0 through 9
```

## `PongR`: alternate halves

`PongR` alternates between the lower and upper halves of a range on successive calls. The first call samples the lower half. The integer and long overloads maintain **separate** alternation state.

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

`DiceR`, `Dice6F`, and `PerlinNoise` are compiled only under `DEBUG`. `DiceR` is abstract, and these classes have no usable finished randomization or noise implementation. They are not part of the documented release workflow.
