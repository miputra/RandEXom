using System;
using System.Numerics;
using RandEXom.Interface;
using RandEXom.SeedLib;

namespace RandEXom.Framework.Number
{
    /// <summary>Samples values at least Distance away from the previous value.</summary>
    public class DistanceR
    {
        private readonly IRandomR rand;
        private int distance;
        private bool hasLast;
        private long last;

        public DistanceR(int distance = 5) : this(new RandomLib.NetRandom(new SeedR()), distance) { }
        public DistanceR(long seed, int distance = 5) : this(new RandomLib.NetRandom(new SeedR(seed)), distance) { }
        public DistanceR(IRandomR framework, int distance = 5)
        {
            rand = framework ?? throw new ArgumentNullException(nameof(framework));
            SetDistance(distance);
        }

        public virtual void SetDistance(int distance)
        {
            if (distance < 0) throw new ArgumentOutOfRangeException(nameof(distance));
            this.distance = distance;
        }

        public virtual int Next(int min, int max)
        {
            return checked((int)Next((long)min, (long)max));
        }

        public virtual long Next(long min, long max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            long result;
            if (!hasLast || distance == 0)
            {
                result = rand.NextLong(min, max);
            }
            else
            {
                BigInteger lowerEnd = BigInteger.Min(max, (BigInteger)last - distance + 1);
                BigInteger upperStart = BigInteger.Max(min, (BigInteger)last + distance);
                BigInteger lowerCount = BigInteger.Max(0, lowerEnd - min);
                BigInteger upperCount = BigInteger.Max(0, (BigInteger)max - upperStart);
                BigInteger count = lowerCount + upperCount;
                if (count == 0) throw new InvalidOperationException("No value satisfies the requested distance in this range.");
                BigInteger index = Sample(count);
                result = index < lowerCount
                    ? (long)((BigInteger)min + index)
                    : (long)(upperStart + index - lowerCount);
            }
            last = result;
            hasLast = true;
            return result;
        }

        private BigInteger Sample(BigInteger count)
        {
            BigInteger size = BigInteger.One << 64;
            BigInteger bucket = size / count;
            BigInteger limit = bucket * count;
            byte[] bytes = new byte[8];
            ulong value;
            do
            {
                rand.NextBytes(bytes);
                value = BitConverter.ToUInt64(bytes, 0);
            } while ((BigInteger)value >= limit);
            return (BigInteger)value / bucket;
        }
    }
}
