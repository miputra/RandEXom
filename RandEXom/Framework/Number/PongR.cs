// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom


using RandEXom.Interface;
using RandEXom.SeedLib;
using System;
using System.Numerics;

namespace RandEXom.Framework.Number
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
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            if ((long)max - min == 1) return min;
            int_pong *= -1;
            int mid = (int)((long)min + ((long)max - min) / 2);
            return int_pong == 1 ? rand.NextInt(min, mid) : rand.NextInt(mid, max);
        }

        public virtual long Next(long min, long max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            if ((BigInteger)max - min == 1) return min;
            long_pong *= -1;
            long mid = (long)((BigInteger)min + ((BigInteger)max - min) / 2);
            return long_pong == 1 ? rand.NextLong(min, mid) : rand.NextLong(mid, max);
        }
    }
}