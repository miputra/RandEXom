namespace RandEXomBenchmark;

public static class Sources
{
    public static Library Library { get; set; } = null!;
    public static IEnumerable<string> RandomNames => Library.Types.Where(Library.IsRandom).Select(x => x.FullName!);
    public static RandomSource CreateRandom(string name, long seed = 12345) =>
        new(Library.Factory(Library.Find(name), seed)());
}

public sealed class RandomSource
{
    readonly Func<int, int, int> nextInt;
    readonly Func<long, long, long> nextLong;
    readonly Action<byte[]> nextBytes;
    public RandomSource(object instance)
    {
        Type type = instance.GetType();
        nextInt = type.GetMethod("NextInt", new[] { typeof(int), typeof(int) })!.CreateDelegate<Func<int, int, int>>(instance);
        nextLong = type.GetMethod("NextLong", new[] { typeof(long), typeof(long) })!.CreateDelegate<Func<long, long, long>>(instance);
        nextBytes = type.GetMethod("NextBytes", new[] { typeof(byte[]) })!.CreateDelegate<Action<byte[]>>(instance);
    }
    public int NextInt(int min, int max) => nextInt(min, max);
    public long NextLong(long min, long max) => nextLong(min, max);
    public void NextBytes(byte[] bytes) => nextBytes(bytes);
}
