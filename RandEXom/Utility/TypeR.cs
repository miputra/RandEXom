using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.Utility
{
    class TypeR
    {
        /// <summary>
        /// Generate long value so it will not cut off when casted to interger. The return value will be different than the input. Useful for overflowed seed
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long RoundLongToInt(long value)
        {
            return unchecked((int)(value ^ (value >> 32)));
        }
    }
}
