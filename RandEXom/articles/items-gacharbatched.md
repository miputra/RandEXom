# 3.3.3 `GachaRBatched<T>`: count-backed bag

[Parent: 3.3 Framework.Item](items.md) · [Guide map](../index.md)

`GachaRBatched<T>` stores a count for each distinct item rather than a list of every copy. It also draws without replacement, but **does not automatically refill** when empty.

```csharp
using RandEXom.Framework.Item;

var bag = new GachaRBatched<string>(seed: 42);
bag.AddItem("common", 100);
bag.AddItem("rare", 1);

string first = bag.Pull();
bag.Refill(); // restore the initial counts
```

Here `Count()` is the total remaining copies, `Count(item)` is the remaining count for one item, and `CountType()` is the number of distinct keys still present. `Pull()` decrements the selected item's count and removes its key at zero; on an empty bag, it returns `default(T)`.

`Remove(value)` removes **all** copies of that key from both initial and current counts. `RemoveEmpty(item)` records the current counts as the new refill baseline; its `item` argument is unused. `Refill(bool isIterative)` and `RemoveEmpty(item, bool isIterative)` have overloads that copy dictionaries in different ways but produce the same intended pool state. Avoid `null` keys: the dictionary-backed implementation does not accept them.

Use `GachaR<T>` when you want the simple list-backed behavior and optional automatic refill. Use `GachaRBatched<T>` when counts are large and you want to control refilling explicitly. In both versions, a `default(T)` result is ambiguous if that value is also a legitimate item; check `Count()` before drawing when that matters.

---

← Previous: [3.3.2 GachaR<T>: list-backed bag](items-gachar.md) · [Parent: 3.3 Framework.Item](items.md) · Next: [3.4 Number](numbers.md) →
