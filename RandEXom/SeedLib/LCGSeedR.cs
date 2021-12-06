/*
 * 
 *  Based on Numerical Recipes in C The Art of Scientific Computing 2nd ed (https://www.amazon.com/Numerical-Recipes-Scientific-Computing-Second/dp/0521431085)
 *  Equation number 7.1.1
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.SeedLib
{
    /// <summary>
    /// generate seed using Linear Congruential Generators
    /// </summary>
    public class LCGSeedR : RandEXom.Interface.ISeedR
    {
        private long _seed = 0;
        private long currentSeed = 0;
        private long previousSeed = 0;

        private long a = 1664525; //multiplier (based on numerical recipes)
        private long c = 1013904223; //Increment (based on numerical recipes)
        

        //based on wiki of linear congruential generators
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


        public long init
        {
            get
            {
                return _seed;
            }
        }

        public long previous
        {
            get
            {
                return previousSeed;
            }
        }

        public long now
        {
            get
            {
                return currentSeed;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="a">multiplier</param>
        /// <param name="c">Increment</param>
        /// <param name="m">modulo</param>
        public LCGSeedR(long? seed = null, long a = 1664525, long c = 1013904223, long m = 2 ^ 32)
        {
            long new_seed = 0;
            if (seed == null)
                new_seed = Utility.LongR.GetJoinedCurrentDate();
            else
                new_seed = (long)seed;
            this._seed = new_seed;
            this.currentSeed = new_seed;
            this.previousSeed = new_seed;
            this.a = a;
            this.c = c;
            //this.m = m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="template">using template from wiki https://en.wikipedia.org/wiki/Linear_congruential_generator </param>
        public LCGSeedR(ParameterTemplate template, long ? seed = null)
        {
            long new_seed = 0;
            if (seed == null)
                new_seed = Utility.LongR.GetJoinedCurrentDate();
            else
                new_seed = (long)seed;
            this._seed = new_seed;

            this.currentSeed = new_seed;
            this.previousSeed = new_seed;

            switch (template)
            {
                case ParameterTemplate.Numerical_Recipes:
                    this.a = 1664525;
                    this.c = 1013904223;
                    break;
                case ParameterTemplate.Apple_CarbonLib:
                    this.a = 16807;
                    this.c = 0;
                    break;
                case ParameterTemplate.Borland_C:
                    this.a = 22695477;
                    this.c = 1;
                    break;
                case ParameterTemplate.Borland_Delphi:
                    this.a = 134775813;
                    this.c = 1;
                    break;
                case ParameterTemplate.cc65:
                    this.a = 65793;
                    this.c = 4282663;
                    break;
                case ParameterTemplate.cc65_2:
                    this.a = 16843009;
                    this.c = 826366247;
                    break;
                case ParameterTemplate.C_Plus_11:
                    this.a = 48271;
                    this.c = 0;
                    break;
                case ParameterTemplate.GLIBC:
                    this.a = 1103515245;
                    this.c = 12345;
                    break;
                case ParameterTemplate.Java:
                    this.a = 25214903917;
                    this.c = 11;
                    break;
                case ParameterTemplate.Microsoft_Visual_Basic:
                    this.a = 1140671485;
                    this.c = 12820163;
                    break;
                case ParameterTemplate.Microsoft_Visual_C:
                    this.a = 214013;
                    this.c = 2531011;
                    break;
                case ParameterTemplate.MTH_RANDOM:
                    this.a = 69069;
                    this.c = 1;
                    break;
                case ParameterTemplate.POSIX:
                    this.a = 25214903917;
                    this.c = 11;
                    break;
                case ParameterTemplate.random0:
                    this.a = 8121;
                    this.c = 28411;
                    break;
                case ParameterTemplate.RANDU:
                    this.a = 65539;
                    this.c = 0;
                    break;
                case ParameterTemplate.RtlUniform:
                    this.a = 2147483629;
                    this.c = 2147483587;
                    break;
                case ParameterTemplate.Turbo_Pascal:
                    this.a = 134775813;
                    this.c = 1;
                    break;
                case ParameterTemplate.ZX81:
                    this.a = 75;
                    this.c = 74;
                    break;
            }
        }

        public void Next()
        {
            previousSeed = currentSeed;
            currentSeed = (a * currentSeed + c);            
        }
    }
}
