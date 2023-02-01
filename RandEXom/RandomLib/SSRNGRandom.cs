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

        public SSRNGRandom(long ? seed = null, long m = 4294967296)
        {
            this.seed = new SeedLib.LCGSeedR(seed);
        }

        public SSRNGRandom(Interface.ISeedR seed, long m = 4294967296)
        {
            this.seed = seed;
        }

        public SSRNGRandom(ParameterTemplate template, long? seed = null)
        {
            this.seed = new SeedLib.LCGSeedR(seed);
        }

        public SSRNGRandom(Interface.ISeedR seed, ParameterTemplate template)
        {
            this.seed = seed;
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
            }
        }


        public virtual string GetSeedSTR()
        {
            return seed.init.ToString();
        }

        public virtual void NextBytes(byte[] buffers)
        {
            for (int i = 0; i < buffers.Length; i++)
            {
                buffers[i] = (byte)Math.Abs((seed.now % m));
                seed.Next();                
            }
        }

        public virtual int NextInt(int min, int max)
        {
            int val = min + Math.Abs((int)((seed.now % m) % (max - min)));  
            seed.Next();
            return val;
        }

        public virtual long NextLong(long min, long max)
        {
            long val = min + Math.Abs(((seed.now % m) % (max - min))); 
            seed.Next();
            return val;
        }

        public ISeedR GetSeed()
        {
            return this.seed;
        }
    }
}