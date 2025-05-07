using RandEXom.Interface;
using RandEXom.SeedLib;
using System;
using System.Collections.Generic;
using System.Text;
using RandEXom.Framework.Item;

namespace RandEXom.Framework.Boolean
{

    /// <summary>
    /// Do Random of boolean using initial persentage
    /// </summary>
    public class TruePercentageR
    {
        IRandomR rand;
        List<bool> items = new List<bool>();
        float persentage = 0;
        GachaR<bool> gachaR;

        /// <summary>
        /// </summary>
        /// <param name="percentage">the chance of 'true'</param>
        public TruePercentageR(float percentage)
        {
            rand = new RandomLib.NetRandom(new SeedR());
            gachaR = new GachaR<bool>(rand);
            this.persentage = percentage;
        }

        public TruePercentageR(long seed, float persentage)
        {
            rand = new RandomLib.NetRandom(new SeedR(seed));
            gachaR = new GachaR<bool>(rand);
            this.persentage = persentage;
        }

        public TruePercentageR(RandEXom.Interface.IRandomR framework, float persentage)
        {
            rand = framework;
            gachaR = new GachaR<bool>(rand);
            this.persentage = persentage;
        }

        public void CreateItems()
        {
            gachaR.AddItem(true, (int)(persentage * 100));
            gachaR.AddItem(false, 10000 - (int)(persentage * 100));
            gachaR.Shuffle();
        }

        public bool Next()
        {
            return gachaR.Pull();
        }

        public void Reset()
        {
            gachaR.Refill();           
        }
    }
}