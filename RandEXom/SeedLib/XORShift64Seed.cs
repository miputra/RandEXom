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
    public class XORShift64Seed : RandEXom.Interface.ISeedR
    {
        readonly int shift1 = 13;
        readonly int shift2 = 7;
        readonly int shift3 = 17;
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

        public enum Type
        {
            Xorshift64,
            Xorshift64_star
        }

        protected long currentSeed = 0;
        protected long previousSeed = 0;

        public XORShift64Seed(long? seed = null)
        {
            long new_seed = 0;
            if (seed == null)
                new_seed = Utility.LongR.GetJoinedCurrentDate();
            else
                new_seed = (long)seed;
            this._seed = new_seed;
            this.currentSeed = new_seed;
            this.currentSeed = currentSeed == 0 ? currentSeed + 1 : currentSeed;
            this.previousSeed = new_seed;
            //this.m = m;
        }

        public XORShift64Seed(Type type, long? seed = null)
        {
            long new_seed = 0;
            if (seed == null)
                new_seed = Utility.LongR.GetJoinedCurrentDate();
            else
                new_seed = (long)seed;
            this._seed = new_seed;
            this.currentSeed = new_seed;
            this.currentSeed = currentSeed == 0 ? currentSeed + 1 : currentSeed;
            this.previousSeed = new_seed;

            switch (type)
            {
                case Type.Xorshift64:
                    this.shift1 = 13;
                    this.shift2 = 7;
                    this.shift3 = 17;
                    break;
                case Type.Xorshift64_star:
                    this.shift1 = 12;
                    this.shift2 = 25;
                    this.shift3 = 27;
                    break;                
            }
        }

        public XORShift64Seed(int shift1, int shift2, int shift3, long? seed = null)
        {
            long new_seed = 0;
            if (seed == null)
                new_seed = Utility.LongR.GetJoinedCurrentDate();
            else
                new_seed = (long)seed;
            this._seed = new_seed;
            this.currentSeed = new_seed;
            this.currentSeed = currentSeed == 0 ? currentSeed + 1 : currentSeed;
            this.previousSeed = new_seed;

            this.shift1 = shift1;
            this.shift2 = shift2;
            this.shift3 = shift3;
        }

        public virtual void Next()
        {
            previousSeed = currentSeed;
            ulong c = unchecked((ulong)(currentSeed - long.MinValue));
            c ^= c << 13;
            c ^= c >> 7;
            c ^= c << 17;
            currentSeed = unchecked((long)c + long.MinValue);
            currentSeed = currentSeed == 0 ? currentSeed + 1 : currentSeed;
        }
    }
}
