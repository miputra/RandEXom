# 4.1.1 Public: `SeedGenerator.GetJoinedCurrentDate()`

[Parent: 4.1 Utility namespace](utility.md) · [Guide map](../index.md)

This static method builds a `long` by concatenating parts of the *local* current date and time (milliseconds, seconds, minutes, hours, day, month, and two-digit year). It is a convenience for creating a time-derived input seed:

```csharp
using RandEXom.RandomLib;
using RandEXom.Utility;

long seed = SeedGenerator.GetJoinedCurrentDate();
var random = new ModuloRandom(seed);
```

The returned number is not the random sequence itself; `ModuloRandom` still applies its seed and output algorithms. This value is not a unique identifier, high-entropy secret, or cryptographic seed. The implementation uses local clock fields and concatenates their unpadded decimal representations, so different timestamps can potentially form the same digits. Prefer a fixed explicit seed for reproducible tests. Most constructors can choose a time-based seed when you omit one, so you rarely need this helper.

---

← Previous: [4.1 Utility namespace](utility.md) · [Parent: 4.1 Utility namespace](utility.md) · Next: [4.1.2 Internal: TypeR.RoundLongToInt(long)](utility-typer.md) →
