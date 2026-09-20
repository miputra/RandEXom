using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;

namespace RandEXomBenchmark;

public static class PerformanceReport
{
    public sealed record Result(string Method, string Unit, int CallsPerSample, int Samples, double MeanNs,
        double MedianNs, double MinNs, double StdDevNs, double StandardErrorNs, double OperationsPerSecond,
        double AllocatedBytesPerCall, int Gen0, int Gen1, int Gen2, double MinSampleMs, string? Error);
    static long sink;

    public static void Run(Library library, List<Workload> cases, int samples, string output, bool smoke)
    {
        Directory.CreateDirectory(output);
        List<Result> results = new();
        foreach (var test in cases)
        {
            try
            {
                //warm up using separate full instances; removal workloads never time an empty bag
                for (int warmup = 0; warmup < 3; warmup++) RunCalls(test.Setup(), test.CallsPerSample);
                double[] time = new double[samples];
                double[] allocated = new double[samples];
                int[] collections = new int[3];
                for (int sample = 0; sample < samples; sample++)
                {
                    Func<long> draw = test.Setup();
                    int g0 = GC.CollectionCount(0), g1 = GC.CollectionCount(1), g2 = GC.CollectionCount(2);
                    long memory = GC.GetAllocatedBytesForCurrentThread();
                    long start = Stopwatch.GetTimestamp();
                    RunCalls(draw, test.CallsPerSample);
                    long elapsed = Stopwatch.GetTimestamp() - start;
                    allocated[sample] = (GC.GetAllocatedBytesForCurrentThread() - memory) / (double)test.CallsPerSample;
                    time[sample] = elapsed * (1e9 / Stopwatch.Frequency) / test.CallsPerSample;
                    collections[0] += GC.CollectionCount(0) - g0;
                    collections[1] += GC.CollectionCount(1) - g1;
                    collections[2] += GC.CollectionCount(2) - g2;
                }
                double mean = time.Average();
                double deviation = Math.Sqrt(time.Sum(x => (x - mean) * (x - mean)) / (samples - 1));
                var sorted = time.Order().ToArray();
                double median = (sorted[(samples - 1) / 2] + sorted[samples / 2]) / 2;
                results.Add(new(test.Name, test.Unit, test.CallsPerSample, samples, mean, median, time.Min(), deviation,
                    deviation / Math.Sqrt(samples), mean > 0 ? 1e9 / mean : 0, allocated.Average(),
                    collections[0], collections[1], collections[2], time.Min() * test.CallsPerSample / 1e6, null));
                Console.WriteLine(smoke ? "Checked: " + test.Name : $"{test.Name}: {mean:F2} ns/call, {allocated.Average():F2} B/call");
            }
            catch (Exception error)
            {
                results.Add(new(test.Name, test.Unit, test.CallsPerSample, samples, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, error.GetBaseException().Message));
                Console.WriteLine("Failed: " + test.Name + " - " + error.GetBaseException().Message);
            }
        }
        string prefix = smoke ? "smoke" : "performance";
        var report = new
        {
            CreatedUtc = DateTime.UtcNow, Library = library.Path, LibraryVersion = library.Assembly.GetName().Version?.ToString(),
            Sha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(library.Path))),
            Runtime = RuntimeInformation.FrameworkDescription, OS = RuntimeInformation.OSDescription,
            Architecture = RuntimeInformation.ProcessArchitecture.ToString(), ProcessorCount = Environment.ProcessorCount,
            StopwatchFrequency = Stopwatch.Frequency, library.RandomType, library.SeedType, library.ItemCount, library.Arguments,
            Note = "In-process delegate-call measurements. Short samples are noisy; inspect MinSampleMs. No SDK or target project used.",
            Results = results, library.Skipped
        };
        File.WriteAllText(System.IO.Path.Combine(output, prefix + ".json"), JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
        using var csv = new StreamWriter(System.IO.Path.Combine(output, prefix + ".csv"));
        csv.WriteLine("Method,Unit,CallsPerSample,Samples,MeanNs,MedianNs,MinNs,StdDevNs,StandardErrorNs,OperationsPerSecond,AllocatedBytesPerCall,Gen0,Gen1,Gen2,MinSampleMs,Error");
        foreach (var row in results)
            csv.WriteLine(string.Join(",", new object?[] { row.Method, row.Unit, row.CallsPerSample, row.Samples, row.MeanNs,
                row.MedianNs, row.MinNs, row.StdDevNs, row.StandardErrorNs, row.OperationsPerSecond, row.AllocatedBytesPerCall,
                row.Gen0, row.Gen1, row.Gen2, row.MinSampleMs, row.Error }.Select(x =>
                    "\"" + Convert.ToString(x, CultureInfo.InvariantCulture)?.Replace("\"", "\"\"") + "\"")));
        if (results.Count == 0 || results.Any(x => x.Error != null)) throw new InvalidOperationException("No workloads ran or a workload failed; see " + prefix + ".json.");
        Console.WriteLine("Reports: " + System.IO.Path.GetFullPath(output));
    }

    private static void RunCalls(Func<long> draw, int count)
    {
        long value = 0;
        for (int i = 0; i < count; i++) value = unchecked(value + draw());
        Volatile.Write(ref sink, value);
    }
}
