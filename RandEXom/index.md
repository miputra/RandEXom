
# RandEXom documentation

This is the guide map. Follow the chapters in order if RandEXom is new to you; each page also has a **Previous** and **Next** link. The sidebar uses the same chapter numbers, so you can always see which branch a page belongs to.

RandEXom is a .NET Standard 2.1 library. Its main path is **SeedLib state → RandomLib draws → Framework behavior**. `Interface` defines the connections; `Utility` contains support functions. If that map is unfamiliar, begin at 1.1.

## 1. Start here

1. [How the library fits together](articles/architecture.md) — what each namespace owns and how a draw travels through the layers.
2. [Installation](articles/installation.md) — use the release DLL or a project reference.
3. [Getting started](articles/intro.md) — make a first draw and try common helpers.

## 2. Core libraries

1. [SeedLib: state](articles/seed-generators.md) — choose how the seed changes.
2. [RandomLib: draws](articles/random-sources.md) — turn state into ranges and bytes.

## 3. Framework helpers

1. [Framework overview](articles/framework.md) — choose a task-level behavior.
2. [Boolean](articles/boolean.md) — independent percentage outcomes.
3. [Item](articles/items.md) — shuffling and two kinds of gacha bag.
4. [Number](articles/numbers.md) — slot, alternating halves, and minimum distance.

## 4. Reference

1. [Utility](articles/utility.md) — time-derived seed helper and internal conversion.
2. [Interfaces and limitations](articles/extending.md) — plug in your own source or seed and understand the limits.
3. [Research and provenance](articles/research.md) — papers behind algorithms, book-sourced parameters, and non-implemented references.
4. [Class index](api/index.md) — find a type by name and jump to its guide.

Looking for one class rather than a learning path? Use the [class index](api/index.md). The [README](https://github.com/miputra/RandEXom/blob/main/README.md) has release information, verification instructions, and support options.

**Start reading → [1.1 How the library fits together](articles/architecture.md)**
