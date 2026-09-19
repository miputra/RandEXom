# 3.3.1 `Shuffles_FisherYates`: Fisher–Yates shuffle

[Parent: 3.3 Framework.Item](items.md) · [Guide map](../index.md)

`Shuffles_FisherYates.Shuffle<T>` shuffles a `List<T>` **in place**. It does not return a new list.

The in-place swap method is related to Richard Durstenfeld’s [“Algorithm 235: Random permutation” (1964)](https://doi.org/10.1145/364520.364540); this implementation traverses forward rather than backward. The Fisher–Yates name also honors the earlier procedure described in Fisher and Yates’s [1938 book](https://wellcomecollection.org/works/udsvs2rx). See [research and provenance](research.md) for the distinction.

```csharp
using System.Collections.Generic;
using RandEXom.Framework.Item;

var cards = new List<string> { "red", "blue", "green" };
var shuffle = new Shuffles_FisherYates(seed: 42);
shuffle.Shuffle(cards);
```

Make a copy first if you need to preserve the original order.

---

← Previous: [3.3 Framework.Item](items.md) · [Parent: 3.3 Framework.Item](items.md) · Next: [3.3.2 GachaR<T>: list-backed bag](items-gachar.md) →
