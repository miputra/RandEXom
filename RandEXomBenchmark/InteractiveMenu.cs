namespace RandEXomBenchmark;

public static class InteractiveMenu
{
    public static int Run()
    {
        var options = new Dictionary<string, string>
        {
            ["--dll"] = Path.Combine(AppContext.BaseDirectory, "RandEXom.dll"),
            ["--items"] = "256",
            ["--samples"] = "15",
            ["--bytes"] = "262144",
            ["--output"] = Path.Combine(AppContext.BaseDirectory, "results")
        };
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("RandEXom Benchmark");
            Console.WriteLine("DLL: " + options["--dll"]);
            Console.WriteLine("Reports: " + options["--output"]);
            Console.WriteLine("[1] to list available benchmarks");
            Console.WriteLine("[2] to benchmark performance and memory");
            Console.WriteLine("[3] to check randomness quality");
            Console.WriteLine("[4] to run smoke checks");
            Console.WriteLine("[5] to change settings and injection");
            Console.WriteLine("[6] to export random bytes");
            Console.WriteLine("[0] to exit");
            Console.Write("Choose: ");
            string? choice = Console.ReadLine()?.Trim();
            if (choice == null || choice == "0") return 0;
            if (choice == "5") { Settings(options); continue; }
            string? command = choice switch
            {
                "1" => "list", "2" => "performance", "3" => "quality",
                "4" => "smoke", "6" => "export", _ => null
            };
            if (command == null) { Console.WriteLine("Choose a number from the menu."); continue; }

            var current = new Dictionary<string, string>(options);
            if (command == "export")
            {
                Console.Write("Source type (Enter for first discovered source): ");
                string? source = Console.ReadLine();
                if (source == null) return 0;
                if (!string.IsNullOrWhiteSpace(source)) current["--source"] = source.Trim();
                Console.Write("Seed (Enter for 12345): ");
                string? seed = Console.ReadLine();
                if (seed == null) return 0;
                if (!long.TryParse(string.IsNullOrWhiteSpace(seed) ? "12345" : seed, out long value))
                { Console.WriteLine("Seed must be a whole number."); continue; }
                current["--seed"] = value.ToString();
                string path = Path.Combine(AppContext.BaseDirectory, "sample.bin");
                Console.Write("Output file (Enter for " + path + "): ");
                string? output = Console.ReadLine();
                if (output == null) return 0;
                current["--output"] = string.IsNullOrWhiteSpace(output) ? path : output.Trim().Trim('"');
            }
            List<string> args = new() { command };
            foreach (var option in current)
            {
                args.Add(option.Key);
                if (option.Key != "--include-obsolete") args.Add(option.Value);
            }
            Console.WriteLine();
            int result = Program.RunCommand(args.ToArray());
            Console.WriteLine(result == 0 ? "Done." : "Run failed. Check the message above or change settings.");
        }
    }

    private static void Settings(Dictionary<string, string> options)
    {
        string[] keys = { "--dll", "--random", "--seed-type", "--config", "--filter", "--items", "--samples", "--bytes", "--output" };
        string[] names = { "DLL path", "Random implementation", "Seed implementation", "Constructor JSON path", "Type filter", "Item count", "Performance samples", "Quality/export byte count", "Report directory" };
        while (true)
        {
            Console.WriteLine();
            for (int i = 0; i < keys.Length; i++)
                Console.WriteLine("[" + (i + 1) + "] " + names[i] + ": " + options.GetValueOrDefault(keys[i], "(automatic / none)"));
            Console.WriteLine("[10] Include obsolete classes: " + options.ContainsKey("--include-obsolete"));
            Console.WriteLine("[0] Back");
            Console.Write("Choose setting: ");
            string? input = Console.ReadLine();
            if (input == null || input.Trim() == "0") return;
            if (!int.TryParse(input, out int choice) || choice < 1 || choice > 10)
            { Console.WriteLine("Choose a number from the menu."); continue; }
            if (choice == 10)
            {
                if (!options.Remove("--include-obsolete")) options["--include-obsolete"] = "true";
                continue;
            }
            string key = keys[choice - 1];
            if (choice is 2 or 3)
            {
                try
                {
                    var library = new Library(options["--dll"]);
                    var types = library.Types.Where(t => choice == 2 ? Library.IsRandom(t)
                        : t.GetProperty("now") != null && t.GetMethod("Next", Type.EmptyTypes) != null);
                    Console.WriteLine("Available types: " + string.Join(", ", types.Select(t => t.Name)));
                }
                catch (Exception error) { Console.WriteLine("Cannot list types: " + error.GetBaseException().Message); }
            }
            Console.Write(names[choice - 1] + " (Enter to keep" + (choice is >= 2 and <= 5 ? ", - to clear" : "") + "): ");
            string? value = Console.ReadLine()?.Trim().Trim('"');
            if (value == null) return;
            if (value.Length == 0) continue;
            if (value == "-" && choice is >= 2 and <= 5) { options.Remove(key); continue; }
            if (choice is >= 6 and <= 8)
            {
                int min = choice == 7 ? 2 : choice == 8 ? 4096 : 1;
                if (!int.TryParse(value, out int number) || number < min)
                { Console.WriteLine("Enter a whole number of at least " + min + "."); continue; }
            }
            options[key] = value;
        }
    }
}
