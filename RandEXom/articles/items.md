# Item helpers

This branch of [Framework](framework.md) uses random draws to reorder a list or select items from a pool. Its three public helpers live in `RandEXom.Framework.Item`: `Shuffles_FisherYates`, `GachaR<T>`, and `GachaRBatched<T>`. Each can be constructed with a numeric seed or a shared `IRandomR` source. A shared source makes call order matter across helpers. Both gacha variants implement `IGachaR<T>`; shuffle does not.

## Fisher–Yates shuffle

`Shuffles_FisherYates.Shuffle<T>` shuffles a `List<T>` **in place**. It does not return a new list.

```csharp
using System.Collections.Generic;
using RandEXom.Framework.Item;

var cards = new List<string> { "red", "blue", "green" };
var shuffle = new Shuffles_FisherYates(seed: 42);
shuffle.Shuffle(cards);
```

Make a copy first if you need to preserve the original order.

## `GachaR<T>`: list-backed bag

`GachaR<T>` stores each added copy as an element in a list. A draw chooses one remaining element and removes it, so adding more copies gives that item more chances. The default behavior shuffles on additions and refills automatically after the bag empties.

```csharp
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

## `GachaRBatched<T>`: count-backed bag

`GachaRBatched<T>` stores a count for each distinct item rather than a list of every copy. It also draws without replacement, but **does not automatically refill** when empty.

```csharp
var bag = new GachaRBatched<string>(seed: 42);
bag.AddItem("common", 100);
bag.AddItem("rare", 1);

string first = bag.Pull();
bag.Refill(); // restore the initial counts
```

Here `Count()` is the total remaining copies, `Count(item)` is the remaining count for one item, and `CountType()` is the number of distinct keys still present. `Pull()` decrements the selected item's count and removes its key at zero; on an empty bag, it returns `default(T)`.

`Remove(value)` removes **all** copies of that key from both initial and current counts. `RemoveEmpty(item)` records the current counts as the new refill baseline; its `item` argument is unused. `Refill(bool isIterative)` and `RemoveEmpty(item, bool isIterative)` have overloads that copy dictionaries in different ways but produce the same intended pool state. Avoid `null` keys: the dictionary-backed implementation does not accept them.

Use `GachaR<T>` when you want the simple list-backed behavior and optional automatic refill. Use `GachaRBatched<T>` when counts are large and you want to control refilling explicitly. In both versions, a `default(T)` result is ambiguous if that value is also a legitimate item; check `Count()` before drawing when that matters.
