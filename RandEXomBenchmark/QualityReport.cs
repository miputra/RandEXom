using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace RandEXomBenchmark;

public static class QualityReport
{
    public sealed record Statistics(double ByteChiSquare, double EntropyBitsPerByte,
        double OneBitFraction, double MonobitZ, double? RunsZ, double? Lag1Correlation,
        double PairChiSquare, long Runs);

    public sealed record Result(string Source, long Seed, int SampleBytes, Statistics Bytes,
        double IntChiSquare, bool SeedReplayMatches, int RangeFailures);

    public static Statistics Analyze(byte[] bytes)
    {
        if (bytes.Length < 2) throw new ArgumentException("At least two bytes required.");
        long[] histogram = new long[256];
        long[] pairs = new long[256];
        long ones = 0, runs = 0;
        int previous = -1;
        double sx = 0, sy = 0, sxx = 0, syy = 0, sxy = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            byte value = bytes[i];
            histogram[value]++;
            ones += BitOperations.PopCount((uint)value);
            //bits are read most significant first, including across byte boundaries
            for (int bit = 7; bit >= 0; bit--)
            {
                int current = (value >> bit) & 1;
                if (current != previous) runs++;
                previous = current;
            }
            if (i % 2 == 1) pairs[(bytes[i - 1] >> 4) * 16 + (value >> 4)]++;
            if (i > 0)
            {
                double x = bytes[i - 1], y = value;
                sx += x; sy += y; sxx += x * x; syy += y * y; sxy += x * y;
            }
        }
        double entropy = 0;
        foreach (long count in histogram)
            if (count > 0) { double p = (double)count / bytes.Length; entropy -= p * Math.Log2(p); }
        double n = bytes.LongLength * 8, zeros = n - ones;
        double meanRuns = 1 + 2 * ones * zeros / n;
        double varianceRuns = 2 * ones * zeros * (2 * ones * zeros - n) / (n * n * (n - 1));
        double adjacent = bytes.Length - 1;
        double denominator = Math.Sqrt(Math.Max(0, (adjacent * sxx - sx * sx) * (adjacent * syy - sy * sy)));
        return new Statistics(ChiSquare(histogram), entropy, ones / n, (2 * ones - n) / Math.Sqrt(n),
            varianceRuns > 0 ? (runs - meanRuns) / Math.Sqrt(varianceRuns) : null,
            denominator > 0 ? (adjacent * sxy - sx * sy) / denominator : null,
            ChiSquare(pairs), runs);
    }

    private static double ChiSquare(long[] histogram)
    {
        double expected = (double)histogram.Sum() / histogram.Length;
        return histogram.Sum(x => (x - expected) * (x - expected) / expected);
    }

    public static void Run(int samples, string output)
    {
        if (samples < 4096) throw new ArgumentOutOfRangeException(nameof(samples), "Use at least 4096 bytes.");
        Directory.CreateDirectory(output);
        List<Result> results = new();
        foreach (string name in Sources.RandomNames)
        {
            foreach (long seed in new long[] { 1, 12345, 987654321 })
            {
                byte[] bytes = new byte[samples];
                byte[] replay = new byte[samples];
                Sources.CreateRandom(name, seed).NextBytes(bytes);
                Sources.CreateRandom(name, seed).NextBytes(replay);
                var random = Sources.CreateRandom(name, seed);
                long[] bins = new long[256];
                int failures = 0;
                for (int i = 0; i < samples; i++)
                {
                    int draw = random.NextInt(0, 256);
                    if (draw < 0 || draw >= 256) failures++;
                    else bins[draw]++;
                    long wide = random.NextLong(long.MinValue, long.MaxValue);
                    if (wide == long.MaxValue) failures++;
                }
                results.Add(new Result(name, seed, samples, Analyze(bytes), ChiSquare(bins),
                    bytes.SequenceEqual(replay), failures));
            }
            Console.WriteLine("Quality checked: " + name);
        }
        var report = new
        {
            CreatedUtc = DateTime.UtcNow, Runtime = RuntimeInformation.FrameworkDescription,
            Library = Sources.Library.Path, LibraryVersion = Sources.Library.Assembly.GetName().Version?.ToString(),
            Sha256 = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(Sources.Library.Path))),
            Sources.Library.SeedType, Sources.Library.Arguments,
            OS = RuntimeInformation.OSDescription, Architecture = RuntimeInformation.ProcessArchitecture.ToString(),
            ProcessorCount = Environment.ProcessorCount, SampleBytes = samples,
            Note = "Descriptive diagnostics, not a complete NIST/TestU01/PractRand battery or a security certification.",
            Results = results
        };
        File.WriteAllText(Path.Combine(output, "quality.json"), JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
        using var csv = new StreamWriter(Path.Combine(output, "quality.csv"));
        csv.WriteLine("Source,Seed,SampleBytes,ByteChiSquare,EntropyBitsPerByte,OneBitFraction,MonobitZ,RunsZ,Lag1Correlation,PairChiSquare,IntChiSquare,SeedReplayMatches,RangeFailures");
        foreach (var row in results)
            csv.WriteLine(string.Join(",", new object?[] { row.Source, row.Seed, row.SampleBytes,
                row.Bytes.ByteChiSquare, row.Bytes.EntropyBitsPerByte, row.Bytes.OneBitFraction,
                row.Bytes.MonobitZ, row.Bytes.RunsZ, row.Bytes.Lag1Correlation, row.Bytes.PairChiSquare,
                row.IntChiSquare, row.SeedReplayMatches, row.RangeFailures }
                .Select(x => Convert.ToString(x, CultureInfo.InvariantCulture))));
        Console.WriteLine("Reports: " + Path.GetFullPath(output));
        if (results.Count == 0 || results.Any(x => !x.SeedReplayMatches || x.RangeFailures != 0))
            throw new InvalidOperationException("A source failed replay or range checks. See quality.json.");
    }

    public static void Export(string source, long seed, long count, string path)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        var random = Sources.CreateRandom(source, seed);
        //create a new file so an existing result cannot be overwritten accidentally
        using var file = new FileStream(path, FileMode.CreateNew);
        byte[] bytes = new byte[65536];
        while (count > 0)
        {
            if (count < bytes.Length) bytes = new byte[(int)count];
            random.NextBytes(bytes);
            file.Write(bytes);
            count -= bytes.Length;
        }
    }
}
