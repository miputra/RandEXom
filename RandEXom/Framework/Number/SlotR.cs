using System;
using RandEXom.Interface;
using RandEXom.SeedLib;

namespace RandEXom.Framework.Number
{
    /// <summary>Generates an integer in the requested range.</summary>
    public class SlotR
    {
        private readonly IRandomR rand;
        public SlotR() : this(new RandomLib.NetRandom(new SeedR())) { }
        public SlotR(long seed) : this(new RandomLib.NetRandom(new SeedR(seed))) { }
        public SlotR(IRandomR framework)
        {
            rand = framework ?? throw new ArgumentNullException(nameof(framework));
        }

        public virtual int Next(int min, int max)
        {
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            return rand.NextInt(min, max);
        }
    }
}
