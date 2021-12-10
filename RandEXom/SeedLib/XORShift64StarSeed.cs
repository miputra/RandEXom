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

namespace RandEXom.SeedLib
{
    public class XORShift64StarSeed : XORShift64Seed
    {
        public XORShift64StarSeed(long ? seed) : base(seed)
        {
        }

        public override void Next()
        {
            UInt64 c = unchecked((ulong)(currentSeed - long.MinValue));  /* The state must be seeded with a nonzero value. */
            c ^= c >> 12; // a
            c ^= c << 25; // b
            c ^= c >> 27; // c
            c = c * 2685821657736338717;
            this.currentSeed = unchecked((long)c + long.MinValue);
            this.currentSeed = currentSeed == 0 ? currentSeed + 1 : currentSeed;
        }
    }
}
