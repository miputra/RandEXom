using System.Text.Json;

namespace RandEXomBenchmark;

public static class Program
{
    public static int Main(string[] args)
    {
        return args.Length == 0 ? InteractiveMenu.Run() : RunCommand(args);
    }

    public static int RunCommand(string[] args)
    {
        try
        {
            if (args[0] is "help" or "--help")
            {
                Console.WriteLine("Launch without arguments for the interactive menu.");
                Console.WriteLine("RandEXomBenchmark <list|performance|quality|smoke|export> [options]");
                Console.WriteLine("--dll <path>          DLL to load (default: RandEXom.dll beside exe)");
                Console.WriteLine("--random <type>       Random implementation injected into frameworks");
                Console.WriteLine("--seed-type <type>    Seed implementation injected into random sources");
                Console.WriteLine("--config <json>       Constructor parameter values by name");
                Console.WriteLine("--filter <text>       Type-name filter (list/performance/smoke)");
                Console.WriteLine("--items <count>       Item pool size (default 256)");
                Console.WriteLine("--samples <count>     Measurement samples (default 15, minimum 2)");
                Console.WriteLine("--bytes <count>       Quality sample/export size (default 262144)");
                Console.WriteLine("--output <path>       Report directory; export file for export mode");
                Console.WriteLine("--source <type>       Random source for export; --seed <number> (default 12345)");
                Console.WriteLine("--include-obsolete    Explicitly include obsolete classes");
                return 0;
            }
            var options = new Dictionary<string, string>();
            string[] allowed = { "--dll", "--random", "--seed-type", "--config", "--filter", "--items", "--samples", "--bytes", "--output", "--source", "--seed" };
            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] == "--include-obsolete") { options.Add(args[i], "true"); continue; }
                if (!allowed.Contains(args[i]) || i + 1 == args.Length) throw new ArgumentException("Unknown option or missing value: " + args[i]);
                options.Add(args[i], args[++i]);
            }
            string Get(string name, string fallback) => options.GetValueOrDefault(name, fallback);
            int items = int.Parse(Get("--items", "256"));
            int samples = int.Parse(Get("--samples", "15"));
            if (items < 1 || samples < 2) throw new ArgumentException("Items must be positive and samples at least two.");
            var library = new Library(Get("--dll", Path.Combine(AppContext.BaseDirectory, "RandEXom.dll")))
            {
                ItemCount = items, RandomType = options.GetValueOrDefault("--random"), SeedType = options.GetValueOrDefault("--seed-type")
            };
            if (options.TryGetValue("--config", out string? config))
            {
                library.Arguments = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(File.ReadAllText(config))
                    ?? throw new ArgumentException("Constructor configuration must be a JSON object.");
                if (new[] { "seed", "items", "remove_on_pull", "reset_on_empty" }.Any(library.Arguments.ContainsKey))
                    throw new ArgumentException("seed, items, remove_on_pull and reset_on_empty are controlled by the runner.");
            }
            if (library.RandomType != null) library.Find(library.RandomType);
            if (library.SeedType != null) library.Find(library.SeedType);
            Sources.Library = library;
            string output = Get("--output", Path.Combine(AppContext.BaseDirectory, "results"));
            switch (args[0])
            {
                case "list":
                case "performance":
                case "smoke":
                    var cases = library.Discover(Get("--filter", ""), options.ContainsKey("--include-obsolete"));
                    if (args[0] == "list")
                    {
                        foreach (var test in cases) Console.WriteLine(test.Name);
                        foreach (string skipped in library.Skipped.Distinct()) Console.WriteLine("Skipped: " + skipped);
                        return cases.Count > 0 ? 0 : 1;
                    }
                    if (args[0] == "smoke") CheckStatistics();
                    PerformanceReport.Run(library, cases, args[0] == "smoke" ? 2 : samples, output, args[0] == "smoke");
                    break;
                case "quality":
                    QualityReport.Run(int.Parse(Get("--bytes", "262144")), output);
                    break;
                case "export":
                    QualityReport.Export(Get("--source", Sources.RandomNames.First()), long.Parse(Get("--seed", "12345")),
                        long.Parse(Get("--bytes", "262144")), output);
                    break;
                default: throw new ArgumentException("Unknown command. Use --help.");
            }
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error.GetBaseException().Message);
            return 1;
        }
    }

    private static void CheckStatistics()
    {
        var constant = QualityReport.Analyze(new byte[4096]);
        var alternating = QualityReport.Analyze(Enumerable.Repeat((byte)0xAA, 4096).ToArray());
        var uniform = QualityReport.Analyze(Enumerable.Range(0, 4096).Select(x => (byte)x).ToArray());
        if (constant.EntropyBitsPerByte != 0 || constant.Runs != 1 || constant.Lag1Correlation != null
            || alternating.MonobitZ != 0 || alternating.Runs != 32768
            || uniform.ByteChiSquare != 0 || uniform.EntropyBitsPerByte != 8 || !(uniform.Lag1Correlation > 0.9))
            throw new InvalidOperationException("Statistical calculation self-check failed.");
    }
}
