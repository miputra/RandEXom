using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.Utility
{
    class Type
    {
        /// <summary>
        /// Used when random system doesnt support long. The type data will still long, but the length will be interger
        /// That mean it will not cut off when casted to interger
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static long RoundLongToInt(long seed)
        {
            if (seed <= int.MaxValue)
                return seed;

            int new_seed = 0;

            while (seed > int.MaxValue)
            {
                new_seed += int.MaxValue;
                seed -= int.MaxValue;
            }
            new_seed += (int)seed;
            return new_seed;
        }
    }
}
