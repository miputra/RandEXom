# 3.4.2 `PongR`: alternate halves

[Parent: 3.4 Framework.Number](numbers.md) · [Guide map](../index.md)

`PongR` alternates between the lower and upper halves of a range on successive calls. Basically it is like a pong ball where bounce high and low. The first call samples the lower half. The integer and long overloads maintain **separate** alternation state.

```csharp
using RandEXom.Framework.Number;

var pong = new PongR(seed: 42);
int lower = pong.Next(0, 10); // 0 through 4
int upper = pong.Next(0, 10); // 5 through 9
```

For odd-sized ranges, the upper half contains one extra value. For a one-value range, every call returns that value without changing the alternation state. Changing ranges between calls does not reset the alternating half.

---

← Previous: [3.4.1 SlotR: spin one reel at a time](numbers-slotr.md) · [Parent: 3.4 Framework.Number](numbers.md) · Next: [3.4.3 DistanceR: keep draws apart](numbers-distancer.md) →
