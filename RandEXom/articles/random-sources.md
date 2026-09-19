# 2.2 RandomLib: random sources

Chapter 2: Core libraries · Section 2 · [Guide map](../index.md)

`RandomLib` is the **draw layer** between `SeedLib` and `Framework`. A source implements `RandEXom.Interface.IRandomR`: it turns state into integer ranges or bytes and can be passed to a framework helper. You can also call it directly. See the [layer diagram](architecture.md), [seed progression choices](seed-generators.md), and [class index](../api/index.md).

| Source | Built-in seed behavior | Main distinction |
| --- | --- | --- |
| `ModuloRandom` | Creates `XORShift64Seed` unless given an `ISeedR` | Raw xorshift-based output with bounded sampling |
| `NetRandom` | Creates constant internal `SeedR` unless given an `ISeedR` | Wraps `System.Random`; dynamic seeds can trigger reseeding |
| `SSRNGRandom` | Creates `LCGSeedR` unless given an `ISeedR` | Historical LCG output moduli and presets |

All expose `NextInt(min, max)`, `NextLong(min, max)`, `NextBytes(buffer)`, `GetSeed()`, and `GetSeedSTR()`. For the range methods, `min` is included and `max` is excluded. Always pass `min < max`; behavior for an empty range differs between implementations. The methods advance internal state, so repeated calls usually produce different results.

```csharp
using RandEXom.RandomLib;

var random = new ModuloRandom(seed: 12345);
int die = random.NextInt(1, 7);
long sample = random.NextLong(-1000, 1000);
byte[] bytes = new byte[16];
random.NextBytes(bytes);
```

The byte array is filled in place. `GetSeedSTR()` returns the initial seed as text, not a serialized snapshot of the generator's current state. A fixed seed can reproduce a sequence with the same generator and library version, but v1.5b changed some sequences from earlier releases.

## In this chapter

- <a id="modulorandom"></a>[2.2.1 ModuloRandom](random-sources-modulorandom.md)
- <a id="netrandom"></a>[2.2.2 NetRandom](random-sources-netrandom.md)
- <a id="ssrngrandom"></a>[2.2.3 SSRNGRandom](random-sources-ssrngrandom.md)
- <a id="choosing-a-source"></a>[2.2.4 Choosing a source](random-sources-choosing-a-source.md)

---

← Previous: [2.1.5 Constant seed (SeedR)](seed-generators-seedr.md) · [Guide map](../index.md) · Next: [2.2.1 ModuloRandom](random-sources-modulorandom.md) →