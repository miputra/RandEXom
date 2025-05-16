//Based on https://digital.library.adelaide.edu.au/items/8d93d3ef-a4d1-4c27-b976-5bcdf3cce5eb

using RandEXom.SeedLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandEXom.Framework.Item
{
    public class Shuffles_FisherYates
    {

        
        RandEXom.Interface.IRandomR rand;

        public Shuffles_FisherYates()
        {
            rand = new RandomLib.NetRandom(new SeedR());
        }
        public Shuffles_FisherYates(long seed)
        {
            rand = new RandomLib.NetRandom(new SeedR(seed));
        }

        public Shuffles_FisherYates(RandEXom.Interface.IRandomR framework)
        {
            rand = framework;
        }


        public void Shuffle<T>(List<T> items)
        {
            int n = items.Count;
            for (int i = 0; i < (n - 1); i++)
            {
                
                int r = i + rand.NextInt(0,n - i);
                T t = items[r];
                items[r] = items[i];
                items[i] = t;
            }
        }
    }
}
