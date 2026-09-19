# 1.2 Installation

Chapter 1: Start here · Section 2 · [Guide map](../index.md)

RandEXom v1.5b targets **.NET Standard 2.1**. Use it from a project whose runtime supports that target. The repository's verification project targets .NET 8. RandEXom is currently distributed as a DLL and a GitHub release, not as a NuGet package.

<a id="use-the-published-dll"></a>

## 1.2.1 Use the published DLL

1. Download [RandEXom.dll v1.5b](https://miputra.github.io/RandEXom/downloads/RandEXom.dll) or the asset from the [v1.5b release](https://github.com/miputra/RandEXom/releases/tag/1.5b).
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

The published DLL on the project site has SHA-256 `148f2e0f5e3ca3512d41fe1c47ecf0e9e06384b41115566fe27546b99dd6a1e7`. Compare the hash after downloading if you need to confirm you received that exact site artifact.

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

---

← Previous: [1.1 How the library fits together](architecture.md) · [Guide map](../index.md) · Next: [1.3 Getting started](intro.md) →
