// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom

using System;
using System.Collections.Generic;
using System.Linq;
using RandEXom.Interface;
using RandEXom.SeedLib;

namespace RandEXom.Framework
{
    public class DistanceR
    {
        IRandomR rand;
        int distance = 5;
        bool unassigned = false;
        int int_last = 0;
        long long_last = 0;

        public DistanceR(int distance = 5)
        {
            rand = new RandomLib.NetRandom(new SeedR());
            this.distance = distance;
        }
        public DistanceR(long? seed, int distance = 5)
        {
            rand = new RandomLib.NetRandom(new SeedR(seed));
            this.distance = distance;
        }

        public DistanceR(RandEXom.Interface.IRandomR framework, int distance = 5)
        {
            rand = framework;
            this.distance = distance;
        }

        public virtual void SetDistance(int distance)
        {
            this.distance = distance;
        }

        public virtual int Next(int min, int max)
        {
            int x = 0;
            if (!unassigned)
            {
                unassigned = true;
                x = rand.NextInt(min, max);
                int_last = x;
                long_last = int_last;
                return x;
            }
            if (max - min - 1 <= distance)
            {
                x = rand.NextInt(min, max);
                int_last = x;
                return x;
            }

            int[] e = ExcludeInt();
            List<int> y = new List<int>();

            foreach(int z in e)
            {
                if (Enumerable.Range(min, max).Contains(z))
                {
                    min = min == z ? min + 1 : min;
                    max = max - 1 == z ? max - 1 : max;
                    if (!y.Contains(z))
                    {
                        y.Add(z);
                    }
                }
            }

            if (!y.Contains(min))
                y.Add(min);
            if(!y.Contains(max))
                y.Add(max);

            y.Sort();

            int[] r = new int[y.Count];

            for(int i=0; i<y.Count-1; i++)
            {
                r[i] = rand.NextInt(y[i],y[i+1]);
            }
            GachaRBatched<int> gacha = new GachaRBatched<int>(rand);
            for (int i = 0; i < r.Count() - 1; i++)
            {
                gacha.AddItem(r[i], (int)Math.Abs(y[i] - y[i + 1]));
            }
            int result = gacha.Pull();
            int_last = result;
            return result;
        }

        public virtual long Next(long min, long max)
        {
            long x = 0;
            if (!unassigned)
            {
                unassigned = true;
                x = rand.NextLong(min, max);
                long_last = x;
                int_last = long_last > int.MaxValue ? int.MaxValue : (int)x;
                return x;
            }
            if (max - min - 1 <= distance)
            {
                x = rand.NextLong(min, max);
                long_last = x;
                return x;
            }

            long[] e = ExcludeLong();
            List<long> y = new List<long>();

            foreach (int z in e)
            {
                if (z >= min && z < max)
                {
                    min = min == z ? min + 1 : min;
                    max = max - 1 == z ? max - 1 : max;
                    if (!y.Contains(z))
                    {
                        y.Add(z);
                    }
                }
            }

            if (!y.Contains(min))
                y.Add(min);
            if (!y.Contains(max))
                y.Add(max);
            y.Sort();
            
            long[] r = new long[y.Count];

            for (int i = 0; i < y.Count - 1; i++)
            {
                r[i] = rand.NextLong(y[i], y[i + 1]);
            }
            GachaRBatched<long> gacha = new GachaRBatched<long>(rand);
            for (int i = 0; i < r.Count() - 1; i++)
            {
                gacha.AddItem(r[i], (int) Math.Abs(y[i] - y[i + 1]));
            }
            long result = gacha.Pull();
            long_last = result;
            return result;
        }

        private int[] ExcludeInt()
        {
            int[] e = new int[4];
            
            int t1 = 0;
            int t2 = 0;
            int t3 = 0;
            int t4 = 0;

            //check overflow
            try
            {
                checked
                {
                    t1 = int_last + distance;
                }
            }
            catch
            {
                t1 = int.MaxValue;
            }
            try
            {
                checked
                {
                    t2 = int_last - distance;
                }
            }
            catch
            {
                t2 = int.MinValue;
            }

            if (long_last > int.MaxValue)
            {
                t3 = int.MaxValue;
            }
            else
            {
                try
                {
                    for (int i = 0; i <= distance; i++)
                    {
                        checked
                        {
                            int temp = (int)long_last + i;
                            t3 = temp;
                        }
                    }
                }
                catch
                {
                }
            }

            if (long_last < int.MinValue)
            {
                t4 = int.MinValue;
            }
            else
            {
                try
                {
                    for (int i = 0; i <= distance; i++)
                    {
                        checked
                        {
                            int temp = (int)long_last - i;
                            t4 = temp;
                        }
                    }
                }
                catch
                {
                }
            }

            e[0] = t1;
            e[1] = t2;
            e[2] = t3;
            e[3] = t4;
            return e;
        }

        private long[] ExcludeLong()
        {
            long[] e = new long[4];

            long t1 = 0;
            long t2 = 0;

            //check overflow
            try
            {
                checked
                {
                    t1 = long_last + distance;
                }
            }
            catch
            {
                t1 = long.MaxValue;
            }
            try
            {
                checked
                {
                    t2 = long_last - distance;
                }
            }
            catch
            {
                t2 = long.MinValue;
            }          

            e[0] = int_last + distance;
            e[1] = int_last - distance;
            e[2] = t1;
            e[3] = t2;
            Array.Sort(e);            
            return e;
        }
    }
}