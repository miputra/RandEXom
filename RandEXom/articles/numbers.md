# 3.4 Framework.Number

Chapter 3: Framework helpers · Section 4 · [Guide map](../index.md)

This branch of [Framework](framework.md) adds different rules to numeric draws. Its types are `SlotR` (one draw per slot-machine reel), `PongR` (alternating halves), and `DistanceR` (gap from the last result). They live in `RandEXom.Framework.Number` and accept either a seed number or an `IRandomR` source. Their range methods include the minimum and exclude the maximum. For a tree that picks items, see [DistributedTreeR<T>](items-distributedtreer.md) in Framework.Item.

## In this chapter

- <a id="slotr-spin-one-reel-at-a-time"></a>[3.4.1 SlotR: spin one reel at a time](numbers-slotr.md)
- <a id="pongr-alternate-halves"></a>[3.4.2 PongR: alternate halves](numbers-pongr.md)
- <a id="distancer-keep-draws-apart"></a>[3.4.3 DistanceR: keep draws apart](numbers-distancer.md)

---

← Previous: [3.3.4 DistributedTreeR<T>](items-distributedtreer.md) · [Guide map](../index.md) · Next: [3.4.1 SlotR: spin one reel at a time](numbers-slotr.md) →
