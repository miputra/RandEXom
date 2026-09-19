# 3.3.2 `GachaR<T>`: list-backed bag

[Parent: 3.3 Framework.Item](items.md) · [Guide map](../index.md)

`GachaR<T>` stores each added copy as an element in a list. A draw chooses one remaining element and removes it, so adding more copies gives that item more chances. The default behavior shuffles on additions and refills automatically after the bag empties.

```csharp
using RandEXom.Framework.Item;

var bag = new GachaR<string>(seed: 42, reset_on_empty: false);
bag.AddItem("common", 3);
bag.AddItem("rare", 1);

string first = bag.Pull();
int remaining = bag.Count();
int rareRemaining = bag.Count("rare");
int distinctRemaining = bag.CountType();
```

`AddItem(item, count)` requires `count > 0`. With `reset_on_empty: false`, an empty `Pull()` returns `default(T)` (for example `null` for strings). With the default `reset_on_empty: true`, it refills from the initial contents and draws again. An empty initial bag still returns `default(T)`.

`Refill()` restores the initial contents. `Remove(value)` removes one matching copy from both the initial and current lists. `ToList()` returns a copy of the current list. `Shuffle()` shuffles the current list. `RemoveEmpty(item)` replaces the initial list with the current contents; its `item` argument is currently unused by the implementation.

This variant stores all copies, so a very large count consumes correspondingly more memory.

---

← Previous: [3.3.1 Shuffles_FisherYates: Fisher–Yates shuffle](items-shuffles-fisheryates.md) · [Parent: 3.3 Framework.Item](items.md) · Next: [3.3.3 GachaRBatched<T>: count-backed bag](items-gacharbatched.md) →
