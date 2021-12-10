/*
 * 
 * 
 * based on research by Sebastiano Vigna https://vigna.di.unimi.it/ftp/papers/xorshift.pdf
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.RandomLib
{
    public class XORShiftStarRandom : RandEXom.Interface.IRandomR
    {
        private RandEXom.Interface.ISeedR seed;
        public XORShiftStarRandom(long? seed = null)
        {
            this.seed = new SeedLib.XORShift64StarSeed(seed);
        }

        public XORShiftStarRandom(Interface.ISeedR seed)
        {
            this.seed = seed;
        }

        public virtual string GetSeed()
        {
            return seed.init.ToString();
        }

        public virtual void NextBytes(byte[] buffers)
        {
            for (int i = 0; i < buffers.Length; i++)
            {
                long s = seed.now * 2685821657736338717;
                buffers[i] = (byte)(Math.Abs((s)) % byte.MaxValue);
                seed.Next();
            }
        }

        public virtual int NextInt(int min, int max)
        {
            long s = seed.now * 2685821657736338717;
            int val = min + (int)Math.Abs(s % (max - min));
            seed.Next();
            return val;
        }

        public virtual long NextLong(long min, long max)
        {
            long s = seed.now * 2685821657736338717;
            long val = min + Math.Abs((s % (max - min)));
            seed.Next();
            return val;
        }
    }
}