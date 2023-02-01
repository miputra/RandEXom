// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom


namespace RandEXom.Interface
{
    public interface IRandomR
    {
        string GetSeedSTR();
        RandEXom.Interface.ISeedR GetSeed();
        int NextInt(int min, int max);
        void NextBytes(byte[] buffers);
        long NextLong(long min, long max);

    }
}
