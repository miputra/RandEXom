# 3.4 Framework.Number

Chapter 3: Framework helpers · Section 4 · [Guide map](../index.md)

This branch of [Framework](framework.md) adds different rules to numeric draws. Its usable types are `SlotR` (one draw per slot-machine reel), `PongR` (alternating halves), and `DistanceR` (gap from the last result). They live in `RandEXom.Framework.Number` and accept either a seed number or an `IRandomR` source. Their range methods include the minimum and exclude the maximum. Experimental tree and unfinished dice/noise types are listed below so their presence in source code is not mistaken for a supported feature.

## In this chapter

- <a id="slotr-spin-one-reel-at-a-time"></a>[3.4.1 SlotR: spin one reel at a time](numbers-slotr.md)
- <a id="pongr-alternate-halves"></a>[3.4.2 PongR: alternate halves](numbers-pongr.md)
- <a id="distancer-keep-draws-apart"></a>[3.4.3 DistanceR: keep draws apart](numbers-distancer.md)
- <a id="experimental-distributed-trees"></a>[3.4.4 Experimental distributed trees](numbers-distributed-trees.md)
- <a id="debug-only-placeholders"></a>[3.4.5 Debug-only placeholders](numbers-debug-placeholders.md)

---

← Previous: [3.3.3 GachaRBatched<T>: count-backed bag](items-gacharbatched.md) · [Guide map](../index.md) · Next: [3.4.1 SlotR: spin one reel at a time](numbers-slotr.md) →