namespace UnrelatedLibrary;

//No reference to the benchmark or RandEXom: discovery must work by public shape.
public interface IRandomSource
{
    int NextInt(int min, int max);
    long NextLong(long min, long max);
    void NextBytes(byte[] bytes);
}

public sealed class NewRandom(long seed = 12345) : IRandomSource
{
    readonly Random random = new((int)seed);
    public int NextInt(int min, int max) => random.Next(min, max);
    public long NextLong(long min, long max) => random.NextInt64(min, max);
    public void NextBytes(byte[] bytes) => random.NextBytes(bytes);
}

public sealed class NewPicker<T>
{
    readonly List<T> items;
    readonly IRandomSource random;
    readonly bool remove;
    public NewPicker(IEnumerable<T> items, IRandomSource framework, bool remove_on_pull = false)
    {
        this.items = new(items);
        random = framework;
        remove = remove_on_pull;
    }
    public T Pull()
    {
        //the benchmark must never warm up or measure removal calls on an empty pool
        if (items.Count == 0) throw new InvalidOperationException("Empty pool was measured.");
        int index = random.NextInt(0, items.Count);
        T result = items[index];
        if (remove) items.RemoveAt(index);
        return result;
    }
    public int Count() => items.Count;
}

public sealed class NeedsConfiguration
{
    public NeedsConfiguration(int customValue)
    {
        if (customValue != 7) throw new ArgumentException("Expected injected configuration value.");
    }
    public int NextInt(int min, int max) => min;
}
