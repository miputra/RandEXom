# 3.4.1 `SlotR`: spin one reel at a time

[Parent: 3.4 Framework.Number](numbers.md) · [Guide map](../index.md)

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

---

← Previous: [3.4 Framework.Number](numbers.md) · [Parent: 3.4 Framework.Number](numbers.md) · Next: [3.4.2 PongR: alternate halves](numbers-pongr.md) →
