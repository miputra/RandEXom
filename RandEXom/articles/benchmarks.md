# 4.6 Benchmarks: measured comparisons

Chapter 4: Reference · Section 6 · [Guide map](../index.md)

These measurements were collected on **20 September 2026** using the standalone DLL-loaded benchmark runner. They describe this local build and workload, not the older downloadable release or every possible seed/source combination.

## Run the benchmark

Download the [Windows x64 benchmark build](https://miputra.github.io/RandEXom/downloads/RandEXomBenchmark-win-x64.zip), extract it, then double-click `RandEXomBenchmark.exe` beside its included `RandEXom.dll`. The menu lists available methods, performance/memory measurements, quality diagnostics, smoke checks, settings/injection, and byte export. Settings let you select a DLL, source, seed type, constructor JSON, item count, samples, and report directory. Command-line use remains available:

```powershell
.\RandEXomBenchmark.exe performance --dll RandEXom.dll --filter RandomLib --samples 25
.\RandEXomBenchmark.exe quality --dll RandEXom.dll --bytes 262144
```

The Windows x64 executable needs the .NET 8 runtime, not an SDK or target project. It discovers compatible public classes/methods and compiles delegates before timing. See the [runner guide in the repository](https://github.com/miputra/RandEXom/blob/main/RandEXomBenchmark/README.md) for publishing, injection rules, supported signatures, and export. Use [TestOut](test-output.md) to watch behavior instead of measuring it.

## Measurement setup

- Release DLL built from the current source; fixed performance seed 12345.
- Windows 10.0.19045, x64 process, .NET 8.0.28, 12 logical processors; stopwatch frequency 10,000,000 ticks/second.
- Three separate process runs for each group, each with three warmup batches and 25 measured samples per workload.
- Tables report the **median of the three per-run medians**, with the smallest/largest per-run median beside it. These ranges are observed variation, not confidence intervals. Allocation columns average the reported managed bytes per call across runs.
- Sources use their default seed algorithms: NetRandom with its internal constant seed, ModuloRandom with XORShift64Seed, SSRNGRandom with default LCG parameters. No custom seed injection was supplied for source comparisons.
- Numeric comparisons use [0, 1,000,000); byte calls fill a reused 1,024-byte buffer. Each measured source sample contains 4,096 calls.
- The desktop was not isolated from other activity. Measurements include delegate/loop overhead and can vary with JIT tiering, GC, and scheduling. Some integer samples are below one millisecond; treat small differences cautiously and inspect `MinSampleMs` in the reports.

DLL assembly version: `0.1.5.0`. SHA-256:

```text
E45424D9CA2E6D66BAAC82135AF55CE8C62943880E4F77D0C7595A732DC282C6
```

## Random-source comparison

| Source | Operation | Median ns/call | Run-median range (ns) | Calls/second from median | Allocated B/call |
| --- | --- | ---: | ---: | ---: | ---: |
| NetRandom | NextInt | 31.54 | 31.10–31.96 | 31702786 | 0.00 |
| NetRandom | NextLong | 75.59 | 74.51–89.16 | 13229974 | 32.00 |
| NetRandom | NextBytes (1,024 bytes) | 4472.58 | 3768.29–4643.51 | 223584 | 0.00 |
| ModuloRandom | NextInt | 483.81 | 393.46–596.00 | 2066912 | 168.00 |
| ModuloRandom | NextLong | 428.56 | 389.72–719.14 | 2333371 | 168.00 |
| ModuloRandom | NextBytes (1,024 bytes) | 4048.68 | 3997.90–4612.35 | 246994 | 0.00 |
| SSRNGRandom | NextInt | 379.69 | 313.62–546.17 | 2633745 | 288.61 |
| SSRNGRandom | NextLong | 395.61 | 366.09–403.37 | 2527771 | 288.61 |
| SSRNGRandom | NextBytes (1,024 bytes) | 336893.07 | 330420.12–340202.69 | 2968 | 294925.12 |

NetRandom had the lowest bounded-int and bounded-long median times in this configuration. ModuloRandom and NetRandom byte-fill timings overlap across runs. SSRNGRandom byte filling incurred substantially more managed allocation here. These results compare complete public APIs, including their range conversion and seed progression, rather than raw generator transitions. To convert byte-fill calls/second to bytes/second, multiply by 1,024.

<a id="item-picker-comparison"></a>

## Item-picker comparison

All rows use 256 distinct integer items, ModuloRandom with its default seed generator, and no initial shuffle. The tree has two branches and three steps (eight endpoints, initially 32 items each). Each sample starts with a fresh full pool outside the timed region and measures exactly 256 pulls; removal samples include endpoint locking as the pool drains. Construction and refill costs are excluded from this table.

| Picker | Behavior | Median ns/pull | Run-median range (ns) | Allocated B/pull |
| --- | --- | ---: | ---: | ---: |
| `GachaR<int>` | Remove | 504.30 | 485.16–650.39 | 168.31 |
| `GachaRBatched<int>` | Remove | 7047.66 | 6807.42–9217.19 | 1252.41 |
| `DistributedTreeR<int>` | Keep | 1694.92 | 1669.53–1724.61 | 704.00 |
| `DistributedTreeR<int>` | Remove and lock | 1785.94 | 1736.33–2630.86 | 699.59 |

GachaR was fastest for this small, distinct-item removal workload. This does not establish the best choice for large repeated weights: GachaRBatched stores counts, while the tree deliberately chooses available branches before items. The keep row permits repeats and is not a without-replacement alternative. Equal branch choices also mean the tree is not a uniformly shuffled bag after endpoints start emptying.

## Randomness diagnostics

Each source was checked with 262,144 bytes and seeds 1, 12345, and 987654321. All nine source/seed cases reproduced their byte sequence and had zero range-check failures. The table shows seed 12345; all seeds, bit-balance/runs statistics, pair-frequency statistics, and integer-distribution statistics are in the downloadable report.

| Source | Byte chi-square | Entropy (bits/byte) | Lag-1 correlation | Int chi-square |
| --- | ---: | ---: | ---: | ---: |
| ModuloRandom | 324.88 | 7.999108 | -0.003292 | 300.24 |
| NetRandom | 274.95 | 7.999242 | 0.002673 | 214.81 |
| SSRNGRandom | 265.68 | 7.999269 | -0.000144 | 252.99 |

For 256 uniform buckets, the chi-square statistic is expected around 255, not zero. Entropy approaching 8 and low lag-1 correlation are descriptive diagnostics, not proof of independence or a quality certification. No p-values or overall pass/fail quality scores are claimed. Constrained helpers and item-removal sequences are not evaluated under the same independent-uniform assumptions.

## Download the measurements

The measurements are unchanged from the runner output; only absolute local DLL paths in JSON were normalized to repository-relative paths. Each JSON includes the DLL hash and environment. These archived results remain a dated snapshot when the code changes.

| Group | Run 1 | Run 2 | Run 3 |
| --- | --- | --- | --- |
| sources | [JSON](benchmark-results/2026-09-20/sources-1/performance.json) · [CSV](benchmark-results/2026-09-20/sources-1/performance.csv) | [JSON](benchmark-results/2026-09-20/sources-2/performance.json) · [CSV](benchmark-results/2026-09-20/sources-2/performance.csv) | [JSON](benchmark-results/2026-09-20/sources-3/performance.json) · [CSV](benchmark-results/2026-09-20/sources-3/performance.csv) |
| items | [JSON](benchmark-results/2026-09-20/items-1/performance.json) · [CSV](benchmark-results/2026-09-20/items-1/performance.csv) | [JSON](benchmark-results/2026-09-20/items-2/performance.json) · [CSV](benchmark-results/2026-09-20/items-2/performance.csv) | [JSON](benchmark-results/2026-09-20/items-3/performance.json) · [CSV](benchmark-results/2026-09-20/items-3/performance.csv) |

Quality: [JSON](benchmark-results/2026-09-20/quality/quality.json) · [CSV](benchmark-results/2026-09-20/quality/quality.csv). Item constructor settings: [items.json](benchmark-results/2026-09-20/items.json).

## Reproduce the comparison

Use the same DLL hash for an exact build comparison, or record the new hash when benchmarking another build. Put the runner, DLL, and downloaded items.json in one folder. Run the source and item commands three times into distinct output folders, then run quality once:

```powershell
foreach ($run in 1..3) {
    .\RandEXomBenchmark.exe performance --dll RandEXom.dll --filter RandomLib --samples 25 --output "results/sources-$run"
    .\RandEXomBenchmark.exe performance --dll RandEXom.dll --filter Framework.Item --random ModuloRandom --items 256 --config items.json --samples 25 --output "results/items-$run"
}
.\RandEXomBenchmark.exe quality --dll RandEXom.dll --bytes 262144 --output results/quality
```

---

← Previous: [4.5 TestOut](test-output.md) · [Guide map](../index.md) · End of guide
