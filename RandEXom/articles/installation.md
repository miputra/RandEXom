# 1.2 Installation

Chapter 1: Start here · Section 2 · [Guide map](../index.md)

RandEXom v1.5b targets **.NET Standard 2.1**. Use it from a project whose runtime supports that target. The repository's verification project targets .NET 8. RandEXom is currently distributed as a DLL and a GitHub release, not as a NuGet package.

<a id="use-the-published-dll"></a>

## 1.2.1 Use the published DLL

1. Download RandEXom.dll from the [latest source build release](https://github.com/miputra/RandEXom/releases/tag/build-2026-09-20).
2. Place the DLL in your project, for example at `lib/RandEXom.dll`.
3. Add a reference in your IDE. In Visual Studio, right-click your project, choose **Add** > **Project Reference** (or **Add Reference**), use **Browse**, and select the DLL. You can also add a file reference directly to your `.csproj`:

   ```xml
   <ItemGroup>
     <Reference Include="RandEXom">
       <HintPath>lib/RandEXom.dll</HintPath>
     </Reference>
   </ItemGroup>
   ```

4. Build your application, then import a namespace such as `RandEXom.RandomLib`.

The latest DLL on the project site has SHA-256 `781585415b14d979b0c787d75f289f12a1492c0ebb4e37dd862ef684b23d719a`. [Checksums for all downloads](https://miputra.github.io/RandEXom/downloads/SHA256SUMS.txt) also cover the TestOut and benchmark packages. These packages target Windows x64 and require the .NET 8 runtime.

<a id="reference-the-source-project"></a>

## 1.2.2 Reference the source project

If you cloned the repository, you can reference the project instead of copying a DLL:

```sh
dotnet add path/to/YourApp.csproj reference path/to/RandEXom/RandEXom.csproj
```

Use paths appropriate to your checkout. A project reference builds RandEXom with your application and follows local source changes; the published DLL keeps your application on the selected release.

<a id="first-compile-check"></a>

## 1.2.3 First compile check

```csharp
using RandEXom.RandomLib;

var random = new ModuloRandom(seed: 12345);
Console.WriteLine(random.NextInt(1, 7)); // one of 1, 2, 3, 4, 5, 6
```

For this top-level-statement example, use a recent C# SDK project. See [getting started](intro.md) for more examples and [random sources](random-sources.md) for choosing a generator.

<a id="build-and-verify-this-repository"></a>

## 1.2.4 Build and verify this repository

From the repository root:

```sh
dotnet build RandEXom.sln
dotnet run --project Verification/Verification.csproj
```

The verification program checks generator sequences and range, percentage, weighted-draw, and distance edge cases. A successful run prints `All verification checks passed.`

For interactive examples, start [TestOut](test-output.md). For speed, allocation, and randomness diagnostics, use the separate DLL-loaded [benchmark runner and comparison results](benchmarks.md).

---

← Previous: [1.1 How the library fits together](architecture.md) · [Guide map](../index.md) · Next: [1.3 Getting started](intro.md) →
