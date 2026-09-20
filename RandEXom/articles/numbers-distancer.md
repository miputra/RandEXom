# 3.4.3 `DistanceR`: keep draws apart

[Parent: 3.4 Framework.Number](numbers.md) · [Guide map](../index.md)

`DistanceR` samples a value at least `distance` away from its **previous** result. The first draw is unrestricted within the range.

```csharp
using RandEXom.Framework.Number;

var spaced = new DistanceR(seed: 42, distance: 3);
int first = spaced.Next(0, 10);
int second = spaced.Next(0, 10); // |second - first| >= 3
```

`distance` must be nonnegative, and `SetDistance()` changes it for future calls. A distance of zero imposes no spacing. If no value in `[min, max)` can satisfy the requested gap, `Next()` throws `InvalidOperationException`. This is not a constraint against *all* earlier values—only the immediately preceding one. `Next` has both `int` and `long` overloads.

---

← Previous: [3.4.2 PongR: alternate halves](numbers-pongr.md) · [Parent: 3.4 Framework.Number](numbers.md) · Next: [4.1 Utility](utility.md) →
