
# RandEXom documentation

This is the guide map. Follow the chapters in order if RandEXom is new to you; each page also has a **Previous** and **Next** link. The sidebar uses the same chapter numbers, so you can always see which branch a page belongs to.

RandEXom is a .NET Standard 2.1 library. Its main path is **SeedLib state → RandomLib draws → Framework behavior**. `Interface` defines the connections; `Utility` contains support functions. If that map is unfamiliar, begin at 1.1.

## 1. Start here

1. [How the library fits together](articles/architecture.md) — what each namespace owns and how a draw travels through the layers.

   - [1.1.1 Namespaces and responsibilities](articles/architecture.md#namespaces-and-responsibilities)
   - [1.1.2 Follow one draw through the layers](articles/architecture.md#follow-one-draw-through-the-layers)
   - [1.1.3 How to choose a path](articles/architecture.md#how-to-choose-a-path)

2. [Installation](articles/installation.md) — use the release DLL or a project reference.

   - [1.2.1 Use the published DLL](articles/installation.md#use-the-published-dll)
   - [1.2.2 Reference the source project](articles/installation.md#reference-the-source-project)
   - [1.2.3 First compile check](articles/installation.md#first-compile-check)
   - [1.2.4 Build and verify this repository](articles/installation.md#build-and-verify-this-repository)

3. [Getting started](articles/intro.md) — make a first draw and try common helpers.

   - [1.3.1 Generate a bounded number](articles/intro.md#generate-a-bounded-number)
   - [1.3.2 Choose a seed generator](articles/intro.md#choose-a-seed-generator)
   - [1.3.3 Shuffle and draw items](articles/intro.md#shuffle-and-draw-items)
   - [1.3.4 Draw a percentage](articles/intro.md#draw-a-percentage)


## 2. Core libraries

1. [SeedLib: state](articles/seed-generators.md) — choose how the seed changes.

   - [2.1.1 XORShift64Seed](articles/seed-generators-xorshift64seed.md)
   - [2.1.2 LCGSeedR](articles/seed-generators-lcgseedr.md)
   - [2.1.3 IterativeSeedR](articles/seed-generators-iterativeseedr.md)
   - [2.1.4 IterativeSeedRCustom](articles/seed-generators-iterativeseedrcustom.md)
   - [2.1.5 Constant seed (SeedR)](articles/seed-generators-seedr.md)

2. [RandomLib: draws](articles/random-sources.md) — turn state into ranges and bytes.

   - [2.2.1 ModuloRandom](articles/random-sources-modulorandom.md)
   - [2.2.2 NetRandom](articles/random-sources-netrandom.md)
   - [2.2.3 SSRNGRandom](articles/random-sources-ssrngrandom.md)
   - [2.2.4 Choosing a source](articles/random-sources-choosing-a-source.md)


## 3. Framework helpers

1. [Framework overview](articles/framework.md) — choose a task-level behavior.

   - [3.1.1 Choose a behavior](articles/framework.md#choose-a-behavior)
   - [3.1.2 Default versus supplied sources](articles/framework.md#default-versus-supplied-sources)

2. [Boolean](articles/boolean.md) — independent percentage outcomes.

   - [3.2.1 TruePercentageR](articles/boolean-truepercentager.md)

3. [Item](articles/items.md) — shuffling and two kinds of gacha bag.

   - [3.3.1 Shuffles_FisherYates: Fisher–Yates shuffle](articles/items-shuffles-fisheryates.md)
   - [3.3.2 GachaR<T>: list-backed bag](articles/items-gachar.md)
   - [3.3.3 GachaRBatched<T>: count-backed bag](articles/items-gacharbatched.md)

4. [Number](articles/numbers.md) — slot, alternating halves, and minimum distance.

   - [3.4.1 SlotR: spin one reel at a time](articles/numbers-slotr.md)
   - [3.4.2 PongR: alternate halves](articles/numbers-pongr.md)
   - [3.4.3 DistanceR: keep draws apart](articles/numbers-distancer.md)
   - [3.4.4 Experimental distributed trees](articles/numbers-distributed-trees.md)
   - [3.4.5 Debug-only placeholders](articles/numbers-debug-placeholders.md)


## 4. Reference

1. [Utility](articles/utility.md) — time-derived seed helper and internal conversion.

   - [4.1.1 Public: SeedGenerator.GetJoinedCurrentDate()](articles/utility-seedgenerator.md)
   - [4.1.2 Internal: TypeR.RoundLongToInt(long)](articles/utility-typer.md)

2. [Interfaces and limitations](articles/extending.md) — plug in your own source or seed and understand the limits.

   - [4.2.1 Pass a custom seed progression](articles/extending.md#pass-a-custom-seed-progression)
   - [4.2.2 Implement an interface](articles/extending.md#implement-an-interface)
   - [4.2.3 Important limits](articles/extending.md#important-limits)
   - [4.2.4 Verification](articles/extending.md#verification)

3. [Research and provenance](articles/research.md) — papers behind algorithms, book-sourced parameters, and non-implemented references.

   - [4.3.1 Implemented algorithm families](articles/research.md#implemented-algorithm-families)
   - [4.3.2 LCG research versus preset provenance](articles/research.md#lcg-research-versus-preset-provenance)
   - [4.3.3 Related research, not an implemented feature](articles/research.md#related-research-not-an-implemented-feature)
   - [4.3.4 Platform behavior and project-specific helpers](articles/research.md#platform-behavior-and-project-specific-helpers)

4. [Class index](api/index.md) — find a type by name and jump to its guide.

Looking for one class rather than a learning path? Use the [class index](api/index.md). The [README](https://github.com/miputra/RandEXom/blob/main/README.md) has release information, verification instructions, and support options.

**Start reading → [1.1 How the library fits together](articles/architecture.md)**
