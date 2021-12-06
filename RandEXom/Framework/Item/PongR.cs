// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom


using RandEXom.Interface;
using RandEXom.SeedLib;

namespace RandEXom.Framework.Item
{
    /// <summary>
    /// Random.Next will always oposite of each other
    /// </summary>
    public class PongR
    {
        IRandomR rand;
        int int_pong = -1;
        int long_pong = -1;

        public PongR()
        {
            rand = new RandomLib.NetRandom(new SeedR());
        }
        public PongR(long seed)
        {
            rand = new RandomLib.NetRandom(new SeedR(seed));
        }

        public PongR(RandEXom.Interface.IRandomR framework)
        {
            rand = framework;
        }

        public virtual int Next(int min, int max)
        {
            int_pong *= -1;
            return int_pong == 1 ? rand.NextInt(min, min + (max-min)/2) : rand.NextInt(min + (max - min) / 2,max);
        }

        public virtual long Next(long min, long max)
        {
            long_pong *= -1;
            return long_pong == 1 ? rand.NextLong(min, min + (max - min) / 2) : rand.NextLong(min + (max - min) / 2, max);
        }
    }
}