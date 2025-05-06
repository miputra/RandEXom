using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.Framework.Number
{
    [ObsoleteAttribute("This class still in development. Expect for some unexpected result.", false)]
    public abstract class DiceR
    {
        public class Number
        {
            int center;
            /// <summary>
            /// 0 is start, where count is end. 1 is the opposite of count - 1
            /// </summary>
            int[] directions;
        }
        public abstract int[] Roll(float power, float shakeTimes);
    }
}
