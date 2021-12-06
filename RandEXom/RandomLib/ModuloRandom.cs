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
        public ModuloRandom(long? seed = null)
        {
            this.seed = new SeedLib.XORShiftSeed(seed);
        }

        public ModuloRandom(Interface.ISeedR seed)
        {
            this.seed = seed;
        }

        public string GetSeed()
        {
            return seed.init.ToString();
        }

        public void NextBytes(byte[] buffers)
        {
            for (int i = 0; i < buffers.Length; i++)
            {
                buffers[i] = (byte)Math.Abs((seed.now));
                seed.Next();
            }
        }

        public int NextInt(int min, int max)
        {
            int val = min + (int)Math.Abs(seed.now % (max - min));
            seed.Next();
            return val;
        }

        public long NextLong(long min, long max)
        {
            long val = min + Math.Abs((seed.now % (max - min)));
            seed.Next();
            return val;
        }
    }
}
