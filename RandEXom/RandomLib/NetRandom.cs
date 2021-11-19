// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom

using System;
using RandEXom.SeedLib;

namespace RandEXom.RandomLib
{
    /// <summary>
    /// Default Random using .Net system
    /// .Net system using substractivegenerator https://referencesource.microsoft.com/#mscorlib/system/random.cs
    /// and the algorithm based on Numerical Recipes in C (2nd Ed.)
    /// The long parameter will converted to int and may cause memory dump
    /// </summary>
    public class NetRandom :RandEXom.Interface.IRandomR
    {
        private RandEXom.Interface.ISeedR seed;
        System.Random net_r;

        public string GetSeed()
        {
            return seed.init.ToString();
        }

        public NetRandom()
        {
            this.seed = new SeedR();
            net_r = new System.Random((int)RandEXom.Utility.TypeR.RoundLongToInt(this.seed.now));
        }


        public NetRandom(long seed)
        {
            this.seed = new SeedR(seed);
            net_r = new System.Random((int)RandEXom.Utility.TypeR.RoundLongToInt(this.seed.now));
        }

        public NetRandom(RandEXom.Interface.ISeedR seed)
        {
            this.seed = seed;
            net_r = new System.Random((int)RandEXom.Utility.TypeR.RoundLongToInt(this.seed.now));
        }      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">inclusive</param>
        /// <param name="max">exclusive</param>
        /// <returns></returns>
        public virtual int NextInt(int min, int max)
        {
            if(seed.previous != seed.now)
                net_r = new System.Random((int)seed.now);
            seed.Next();
            return net_r.Next(min,max);
        }

        public virtual void NextBytes(byte[] buffers)
        {
            if (seed.previous != seed.now)
            {
                this.net_r = new System.Random((int)seed.now);
            }
            seed.Next();
            this.net_r.NextBytes(buffers);
        }

        /// <summary>
        /// this code in particular function is made by Mark Amery from
        /// https://stackoverflow.com/questions/6651554/random-number-in-long-range-is-this-the-way
        /// </summary>
        /// <param name="min">inclusive</param>
        /// <param name="max">exclusive</param>
        /// <returns></returns>
        public virtual long NextLong(long min, long max)
        {
            ulong uRange = (ulong)(max - min);

            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }

        
    }
}
