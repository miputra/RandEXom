using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.SeedLib
{
    /// <summary>
    /// Based on research by George Marsaglia https://www.jstatsoft.org/article/view/v008i14
    /// </summary>
    public class XORShiftSeed : RandEXom.Interface.ISeedR
    {

        private long _seed = 0;
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
        protected long currentSeed = 0;
        protected long previousSeed = 0;

        public XORShiftSeed(long? seed = null)
        {
            long new_seed = 0;
            if (seed == null)
                new_seed = Utility.LongR.GetJoinedCurrentDate();
            else
                new_seed = (long)seed;
            this._seed = new_seed;
            this.currentSeed = new_seed;
            this.previousSeed = new_seed;
            //this.m = m;
        }

        public virtual void Next()
        {
            previousSeed = currentSeed;
            ulong c = unchecked((ulong)(currentSeed - long.MinValue));
            c ^= c << 13;
            c ^= c >> 7;
            c ^= c << 17;
            currentSeed = unchecked((long)c + long.MinValue);
        }
    }
}
