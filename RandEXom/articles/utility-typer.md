# 4.1.2 Internal: `TypeR.RoundLongToInt(long)`

[Parent: 4.1 Utility namespace](utility.md) · [Guide map](../index.md)

`TypeR` is an internal class and **cannot be called from a consuming assembly**. `NetRandom` uses its conversion to fold a 64-bit seed into the 32-bit seed supplied to `System.Random`. As a consequence, distinct `long` seeds may produce the same `NetRandom` initialization. It is not a general-purpose rounding operation despite the method's name.

If you need to control a `NetRandom` sequence, pass a seed to `new NetRandom(42)`; do not try to reference `TypeR` in your application. See [RandomLib](random-sources.md) for the resulting random-source behavior and [SeedLib](seed-generators.md) for changing seed progression.

---

← Previous: [4.1.1 Public: SeedGenerator.GetJoinedCurrentDate()](utility-seedgenerator.md) · [Parent: 4.1 Utility namespace](utility.md) · Next: [4.2 Interfaces and limitations](extending.md) →
