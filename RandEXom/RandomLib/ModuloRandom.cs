using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ModuloRandom(long? seed = null)
        {
            this.seed = new SeedLib.XORShift64Seed(seed);
        }

        public ModuloRandom(long multiplier, long?seed = null)
        {
            this.seed = new SeedLib.XORShift64Seed(seed);
            this.multiplier = multiplier;
        }

        public ModuloRandom(Multiplier multiplier, long?seed = null)
        {
            this.seed = new SeedLib.XORShift64Seed(seed);
            switch (multiplier)
            {
                case Multiplier.One:
                    this.multiplier = 1;
                    break;
                case Multiplier.XORShift64_Star:
                    this.multiplier = 2685821657736338717;
                    break;
            }
        }

        public ModuloRandom(Interface.ISeedR seed, long multiplier = 1)
        {
            this.seed = seed;
            this.multiplier = multiplier;
        }

        public ModuloRandom(Interface.ISeedR seed, Multiplier multiplier)
        {
            this.seed = seed;
            switch (multiplier)
            {
                case Multiplier.One:
                    this.multiplier = 1;
                    break;
                case Multiplier.XORShift64_Star:
                    this.multiplier = 2685821657736338717;
                    break;

            }
        }

        public virtual string GetSeed()
        {
            return seed.init.ToString();
        }

        public virtual void NextBytes(byte[] buffers)
        {
            long seed = this.seed.now * multiplier;

            for (int i = 0; i < buffers.Length; i++)
            {
                buffers[i] = (byte)(Math.Abs((seed)) % byte.MaxValue);
                this.seed.Next();
            }
        }

        public virtual int NextInt(int min, int max)
        {
            long seed = this.seed.now * multiplier;
            int val = min + (int)Math.Abs(seed % (max - min));
            this.seed.Next();
            return val;
        }

        public virtual long NextLong(long min, long max)
        {
            long seed = this.seed.now * multiplier;
            long val = min + Math.Abs((seed % (max - min)));
            this.seed.Next();
            return val;
        }
    }
}
