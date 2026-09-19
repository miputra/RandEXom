# 2.1.5 Constant seed (`SeedR`)

[Parent: 2.1 SeedLib: seed generators](seed-generators.md) · [Guide map](../index.md)

The library also contains `SeedR`, whose `Next()` does not change its value. It is **internal**, so consumer projects cannot instantiate it directly. Default constructors of helpers and `NetRandom` use it internally. If you need a fixed seed with `NetRandom`, use `new NetRandom(42)`.

---

← Previous: [2.1.4 IterativeSeedRCustom](seed-generators-iterativeseedrcustom.md) · [Parent: 2.1 SeedLib: seed generators](seed-generators.md) · Next: [2.2 RandomLib](random-sources.md) →
