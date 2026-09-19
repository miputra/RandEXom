# Utility namespace

`RandEXom.Utility` is supporting code, not a fourth kind of random source. Ordinary users can create a source with an explicit numeric seed and never call a utility method. The namespace currently has one public helper and one internal conversion helper.

## Public: `SeedGenerator.GetJoinedCurrentDate()`

This static method builds a `long` by concatenating parts of the *local* current date and time (milliseconds, seconds, minutes, hours, day, month, and two-digit year). It is a convenience for creating a time-derived input seed:

```csharp
using RandEXom.RandomLib;
using RandEXom.Utility;

long seed = SeedGenerator.GetJoinedCurrentDate();
var random = new ModuloRandom(seed);
```

The returned number is not the random sequence itself; `ModuloRandom` still applies its seed and output algorithms. This value is not a unique identifier, high-entropy secret, or cryptographic seed. The implementation uses local clock fields and concatenates their unpadded decimal representations, so different timestamps can potentially form the same digits. Prefer a fixed explicit seed for reproducible tests. Most constructors can choose a time-based seed when you omit one, so you rarely need this helper.

## Internal: `TypeR.RoundLongToInt(long)`

`TypeR` is an internal class and **cannot be called from a consuming assembly**. `NetRandom` uses its conversion to fold a 64-bit seed into the 32-bit seed supplied to `System.Random`. As a consequence, distinct `long` seeds may produce the same `NetRandom` initialization. It is not a general-purpose rounding operation despite the method's name.

If you need to control a `NetRandom` sequence, pass a seed to `new NetRandom(42)`; do not try to reference `TypeR` in your application. See [RandomLib](random-sources.md) for the resulting random-source behavior and [SeedLib](seed-generators.md) for changing seed progression.
