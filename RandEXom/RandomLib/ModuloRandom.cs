using RandEXom.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RandEXom.RandomLib
{
    /// <summary>
    /// Simple random with output of seed modulo
    /// </summary>
    public class ModuloRandom : RandEXom.Interface.IRandomR
    {
        private RandEXom.Interface.ISeedR seed;

        readonly private long multiplier = 1;

        public enum Multiplier
        {
            One,
            XORShift64_Star
        }

        public ModuloRandom(int seed) : this((long?)seed) { }

        public ModuloRandom(long? seed = null)
        {
            this.seed = new SeedLib.XORShift64Seed(seed);
        }

        public ModuloRandom(long multiplier, long? seed)
        {
            this.seed = new SeedLib.XORShift64Seed(seed);
            this.multiplier = multiplier;
        }

        public ModuloRandom(Multiplier multiplier, long?seed = null)
        {
            this.seed = multiplier == Multiplier.XORShift64_Star
                ? new SeedLib.XORShift64Seed(SeedLib.XORShift64Seed.Type.Xorshift64_star, seed)
                : new SeedLib.XORShift64Seed(seed);
            switch (multiplier)
            {
                case Multiplier.One:
                    this.multiplier = 1;
                    break;
                case Multiplier.XORShift64_Star:
                    this.multiplier = 2685821657736338717;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(multiplier));
            }
        }

        public ModuloRandom(Interface.ISeedR seed, long multiplier = 1)
        {
            this.seed = seed ?? throw new ArgumentNullException(nameof(seed));
            this.multiplier = multiplier;
        }

        public ModuloRandom(Interface.ISeedR seed, Multiplier multiplier)
        {
            this.seed = seed ?? throw new ArgumentNullException(nameof(seed));
            switch (multiplier)
            {
                case Multiplier.One:
                    this.multiplier = 1;
                    break;
                case Multiplier.XORShift64_Star:
                    this.multiplier = 2685821657736338717;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(multiplier));

            }
        }

        public virtual string GetSeedSTR()
        {
            return seed.init.ToString();
        }

        public virtual void NextBytes(byte[] buffers)
        {
            if (buffers == null) throw new ArgumentNullException(nameof(buffers));
            for (int i = 0; i < buffers.Length; i++)
                buffers[i] = (byte)(NextRaw() >> 56);
        }

        public virtual int NextInt(int min, int max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            return (int)((long)min + (long)NextBounded((ulong)((long)max - min)));
        }

        public virtual long NextLong(long min, long max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            return unchecked((long)((ulong)min + NextBounded(unchecked((ulong)(max - min)))));
        }

        private ulong NextRaw()
        {
            seed.Next();
            return unchecked((ulong)seed.now * (ulong)multiplier);
        }

        private ulong NextBounded(ulong span)
        {
            BigInteger size = BigInteger.One << 64;
            BigInteger bucket = size / span;
            BigInteger limit = bucket * span;
            ulong value;
            do { value = NextRaw(); } while ((BigInteger)value >= limit);
            return (ulong)((BigInteger)value / bucket);
        }

        public ISeedR GetSeed()
        {
            return this.seed;
        }
    }
}
