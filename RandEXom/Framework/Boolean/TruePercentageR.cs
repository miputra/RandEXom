using System;
using RandEXom.Interface;
using RandEXom.SeedLib;

namespace RandEXom.Framework.Boolean
{
    /// <summary>Returns true with the specified percentage on each independent draw.</summary>
    public class TruePercentageR
    {
        private const long Scale = 1L << 32;
        private readonly IRandomR rand;
        private readonly float percentage;
        private long threshold;

        public TruePercentageR(float percentage) : this(new RandomLib.NetRandom(new SeedR()), percentage) { }
        public TruePercentageR(long seed, float percentage) : this(new RandomLib.NetRandom(new SeedR(seed)), percentage) { }
        public TruePercentageR(IRandomR framework, float percentage)
        {
            rand = framework ?? throw new ArgumentNullException(nameof(framework));
            if (float.IsNaN(percentage) || percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage));
            this.percentage = percentage;
            CreateItems();
        }

        /// <summary>Recalculates the probability threshold.</summary>
        public void CreateItems()
        {
            threshold = (long)Math.Round((double)percentage / 100 * Scale, MidpointRounding.AwayFromZero);
        }

        public bool Next()
        {
            return rand.NextLong(0, Scale) < threshold;
        }

        /// <summary>Recalculates the probability threshold; draws have no batch state.</summary>
        public void Reset()
        {
            CreateItems();
        }
    }
}
