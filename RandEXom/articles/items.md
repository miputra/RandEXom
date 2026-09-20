# 3.3 Framework.Item

Chapter 3: Framework helpers · Section 3 · [Guide map](../index.md)

This branch of [Framework](framework.md) uses random draws to reorder a list or select items from a pool. Its four public helpers live in `RandEXom.Framework.Item`: `Shuffles_FisherYates`, `GachaR<T>`, `GachaRBatched<T>`, and `DistributedTreeR<T>`. Each can be constructed with a numeric seed or a shared `IRandomR` source. A shared source makes call order matter across helpers. Both gacha variants implement `IGachaR<T>`; shuffle and the tree picker do not.

## In this chapter

- <a id="fisheryates-shuffle"></a>[3.3.1 Shuffles_FisherYates: Fisher–Yates shuffle](items-shuffles-fisheryates.md)
- <a id="gachart-list-backed-bag"></a>[3.3.2 GachaR<T>: list-backed bag](items-gachar.md)
- <a id="gacharbatchedt-count-backed-bag"></a>[3.3.3 GachaRBatched<T>: count-backed bag](items-gacharbatched.md)
- [3.3.4 DistributedTreeR<T>: random branch item picker](items-distributedtreer.md)

---

← Previous: [3.2.1 TruePercentageR](boolean-truepercentager.md) · [Guide map](../index.md) · Next: [3.3.1 Shuffles_FisherYates: Fisher–Yates shuffle](items-shuffles-fisheryates.md) →
