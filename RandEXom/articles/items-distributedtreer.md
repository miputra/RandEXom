# 3.3.4 DistributedTreeR<T>: random branch item picker

`RandEXom.Framework.Item.DistributedTreeR<T>` divides a copy of the input items among endpoints. Each `Pull()` chooses one random branch per step, then a random item inside the selected endpoint. By default items remain available for later pulls. Set `remove_on_pull: true` to remove each selected item from its endpoint.

```csharp
using RandEXom.Framework.Item;
using RandEXom.RandomLib;
using RandEXom.SeedLib;

var random = new ModuloRandom(new XORShift64Seed(123));
var tree = new DistributedTreeR<string>(
    new[] { "A", "B", "C", "D", "E", "F", "G", "H" },
    random, step: 2, child: 2, do_shuffle: false, remove_on_pull: true);

string item = tree.Pull();
```

Here two branch choices lead to four endpoints: `[A, B]`, `[C, D]`, `[E, F]`, and `[G, H]`. Choosing branch 1 followed by branch 0 reaches `[E, F]`. One more random draw selects E or F.

- `step` counts branch choices, so endpoint count is `child` raised to `step`. Zero steps uses one endpoint.
- `child` is the number of branches at every internal node; it must be at least one.
- `do_shuffle: false` preserves input index order. `true` shuffles the copied items once before dividing them, using the supplied random source.
- Endpoint sizes are equal when possible at initialization only. Pulling never redistributes the remaining items. Otherwise the first endpoints receive one extra item, so sizes differ by at most one. For example, 11 items across three endpoints gives sizes 4, 4, 3. Initially endpoints have equal selection probability; items in smaller endpoints therefore have a higher individual probability.
- Empty input and configurations with more endpoints than items are rejected.

Constructors also accept a numeric `seed`, or use the default `NetRandom` source when no source is supplied. `Count()` returns the remaining item count and `ToList()` returns a copy in endpoint order. Input changes and changes to the returned list do not change the tree's item placement.

With `remove_on_pull: true`, an empty endpoint is locked by removing it from its parent's available branches. If that parent has no available branches left, it is removed from its own parent, continuing up the tree. Each draw chooses uniformly among the current node's available branches. Remaining endpoints are not necessarily equally likely after branches are removed. Items stay in their original endpoints.

When all items have been removed, `Pull()` returns `default(T)` without making another random draw or refilling the tree. Use `Count()` to distinguish an empty tree from a stored default value. With `remove_on_pull: false`, items remain available and endpoint sizes stay unchanged.

See [TestOut option 9](test-output.md#item-tree-output) for an interactive demonstration and [benchmark comparisons](benchmarks.md#item-picker-comparison) for measured pull costs.

---

← Previous: [3.3.3 GachaRBatched<T>](items-gacharbatched.md) · [Parent: 3.3 Framework.Item](items.md) · Next: [3.4 Number](numbers.md) →
