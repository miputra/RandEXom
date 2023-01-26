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
    public class ByPersentage
    {
        IRandomR rand;
        List<bool> items = new List<bool>();
        float persentage = 0;
        GachaR<bool> gachaR;

        public ByPersentage(float persentage)
        {
            rand = new RandomLib.NetRandom(new SeedR());
            gachaR = new GachaR<bool>(rand);
            this.persentage = persentage;
        }

        public ByPersentage(long seed, float persentage)
        {
            rand = new RandomLib.NetRandom(new SeedR(seed));
            gachaR = new GachaR<bool>(rand);
            this.persentage = persentage;
        }

        public ByPersentage(RandEXom.Interface.IRandomR framework, float persentage)
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