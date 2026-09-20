# 4.5 TestOut: interactive output examples

Chapter 4: Reference · Section 5 · [Guide map](../index.md)

`RandExOmTestOut` is the console demonstration project. It prints draws so you can see how each helper behaves. The [benchmark runner](benchmarks.md) measures speed, allocations, and statistical diagnostics; [Verification](extending.md#verification) checks correctness cases.

## Open TestOut

Build `RandExOmTestOut` in Visual Studio and start it, or double-click `RandExOmTestOut.exe` in its build/publish folder. Keep its accompanying DLL and runtime files together. It targets .NET 8.

From the repository root, you can also run:

```powershell
dotnet run --project RandExOmTestOut/RandExOmTestOut.csproj
```

Unlike the DLL-loaded benchmark runner, TestOut has a project reference to RandEXom. Building TestOut builds the library too. Enter a menu number and press Enter. After each example finishes, the menu appears again. Enter `0` or any unlisted value to exit; end-of-input also exits.

## Menu options

| Option | Menu label | What it demonstrates |
| --- | --- | --- |
| 1 | gacha | `GachaR<string>` pulls from weighted rarity entries |
| 2 | gachalite | `GachaRBatched<string>` uses counts for the same rarity entries; “gachalite” is the menu label |
| 3 | pong | `PongR` alternates range halves for int and long draws |
| 4 | distance | `DistanceR` keeps a gap from the previous draw, including mixed int/long calls |
| 5 | SSRNG | `SSRNGRandom` bounded int and long output |
| 6 | XORShift | Default `ModuloRandom` output |
| 7 | XORShiftStar | `ModuloRandom` with `XORShift64_Star` |
| 8 | SlotR | Ten bounded draws from Next(0, 100) |
| 9 | Distributed Tree Item | Ordered/shuffled endpoint placement with both keep and remove modes |

Most demonstrations use default seeds, so their output can differ between runs. Console-printing time and the demo's elapsed-time line are not a reliable speed comparison; use the dedicated benchmark runner for that.

<a id="item-tree-output"></a>

## Item-tree output

Option **9** runs all four combinations of `do_shuffle` and `remove_on_pull` for `DistributedTreeR<string>`. Each starts with 12 distinct items, two branches per node, two steps, and `ModuloRandom(new XORShift64Seed(123))`. The four endpoint groups start with three items each.

The ordered removal case begins like this:

```text
Shuffle: False, Remove on pull: True
Endpoint 0: Item1, Item2, Item3
Endpoint 1: Item4, Item5, Item6
Endpoint 2: Item7, Item8, Item9
Endpoint 3: Item10, Item11, Item12
Item5 from endpoint 1, Remaining: 11
Item6 from endpoint 1, Remaining: 10
Item4 from endpoint 1, Remaining: 9
Endpoint 1 is empty and locked.
```

Endpoint numbers are zero-based and refer to the initial placement. In keep mode, all 12 items remain available and repeated values are expected. In removal mode, each item appears once, the remaining count falls to zero, and empty endpoints stop receiving draws. No items move between groups. When a whole branch empties, the picker stops visiting that branch too.

The removal cases finish with:

```text
All endpoints locked. Empty pull returns null.
Test passed.
```

The demo checks unchanged placement in keep mode, unique removed items, remaining counts, and the exhausted result. These checks supplement the more detailed [Verification cases](extending.md#verification), including deterministic branch paths and uneven endpoint sizes. See [the item-tree guide](items-distributedtreer.md) for the API and [measured comparisons](benchmarks.md#item-picker-comparison) for pull costs.

---

← Previous: [4.4 Class index](../api/index.md) · [Guide map](../index.md) · Next: [4.6 Benchmarks](benchmarks.md) →

