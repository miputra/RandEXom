/*
 * 
 *  Based on Numerical Recipes in C The Art of Scientific Computing 2nd ed (https://www.amazon.com/Numerical-Recipes-Scientific-Computing-Second/dp/0521431085)
 *  Equation number 7.1.1
 * 
 */

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
    /// System-Supplied Random Number Generators
    /// </summary>
    public class SSRNGRandom : RandEXom.Interface.IRandomR
    {
        private RandEXom.Interface.ISeedR seed;
        private long m = 4294967296; //modulo (based on numerical recipes)

        public enum ParameterTemplate
        {
            ZX81,
            Numerical_Recipes,
            Borland_C,
            GLIBC,
            IBM,
            Borland_Delphi,
            Turbo_Pascal,
            Microsoft_Visual_C,
            Microsoft_Visual_Basic,
            RtlUniform,
            Apple_CarbonLib,
            C_Plus_11,
            MTH_RANDOM,
            Java,
            random0,
            POSIX,
            cc65,
            cc65_2,
            RANDU
        }

        public SSRNGRandom(int seed) : this((long?)seed) { }

        public SSRNGRandom(long ? seed = null, long m = 4294967296)
        {
            if (m <= 1) throw new ArgumentOutOfRangeException(nameof(m));
            this.seed = new SeedLib.LCGSeedR(seed, m: m);
            this.m = m;
        }

        public SSRNGRandom(Interface.ISeedR seed, long m = 4294967296)
        {
            if (m <= 1) throw new ArgumentOutOfRangeException(nameof(m));
            this.seed = seed ?? throw new ArgumentNullException(nameof(seed));
            this.m = m;
        }

        public SSRNGRandom(ParameterTemplate template, long? seed = null)
        {
            this.seed = new SeedLib.LCGSeedR((SeedLib.LCGSeedR.ParameterTemplate)template, seed);
            SetParameter(template);
        }

        public SSRNGRandom(Interface.ISeedR seed, ParameterTemplate template)
        {
            this.seed = seed ?? throw new ArgumentNullException(nameof(seed));
            SetParameter(template);
        }

        private void SetParameter(ParameterTemplate template)
        {
            switch (template)
            {
                case ParameterTemplate.Numerical_Recipes:
                    this.m = 4294967296; // 2^32
                    break;
                case ParameterTemplate.Apple_CarbonLib:
                    this.m = 2147483647; // 2^31 - 1
                    break;
                case ParameterTemplate.Borland_C:
                    this.m = 4294967296; // 2^32
                    break;
                case ParameterTemplate.Borland_Delphi:
                    this.m = 4294967296; // 2^32
                    break;
                case ParameterTemplate.cc65:
                    this.m = 8388608; // 2^23
                    break;
                case ParameterTemplate.cc65_2:
                    this.m = 4294967296; // 2^32
                    break;
                case ParameterTemplate.C_Plus_11:
                    this.m = 2147483647; //  2^31 - 1
                    break;
                case ParameterTemplate.GLIBC:
                    this.m = 2147483648; //  2^31
                    break;
                case ParameterTemplate.Java:
                    this.m = 281474976710656; //  2^48
                    break;
                case ParameterTemplate.Microsoft_Visual_Basic:
                    this.m = 16777216; //  2^24
                    break;
                case ParameterTemplate.Microsoft_Visual_C:
                    this.m = 4294967296; //  2^32
                    break;
                case ParameterTemplate.MTH_RANDOM:
                    this.m = 4294967296; //  2^32
                    break;
                case ParameterTemplate.POSIX:
                    this.m = 281474976710656; //  2^48
                    break;
                case ParameterTemplate.random0:
                    this.m = 134456; //  2^3 * 7^5
                    break;
                case ParameterTemplate.IBM:
                case ParameterTemplate.RANDU:
                    this.m = 2147483648; //  2^31
                    break;
                case ParameterTemplate.RtlUniform:
                    this.m = 2147483647; //  2^31 - 1
                    break;
                case ParameterTemplate.Turbo_Pascal:
                    this.m = 4294967296; //  2^31 - 1
                    break;
                case ParameterTemplate.ZX81:
                    this.m = 65537; // 2^16 + 1
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(template));
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
            {
                buffers[i] = (byte)NextBounded(256);
            }
        }

        public virtual int NextInt(int min, int max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            return (int)((long)min + (long)NextBounded((ulong)((long)max - min)));
        }

        public virtual long NextLong(long min, long max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            ulong span = unchecked((ulong)(max - min));
            return unchecked((long)((ulong)min + NextBounded(span)));
        }

        private ulong NextBounded(ulong span)
        {
            BigInteger size;
            BigInteger value;
            BigInteger bound = span;
            do
            {
                size = BigInteger.One;
                value = BigInteger.Zero;
                do
                {
                    long digit = seed.now % m;
                    if (digit < 0) digit += m;
                    value = value * m + digit;
                    size *= m;
                    seed.Next();
                } while (size < bound);
            } while (value >= size - size % bound);
            return (ulong)(value / (size / bound));
        }

        public ISeedR GetSeed()
        {
            return this.seed;
        }
    }
}
