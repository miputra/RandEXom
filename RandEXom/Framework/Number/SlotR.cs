

// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom


using RandEXom.Interface;
using RandEXom.SeedLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandEXom.Framework.Number
{
    public class SlotR
    {
        IRandomR rand;
        int int_pong = -1;
        int long_pong = -1;

        public SlotR()
        {
            rand = new RandomLib.NetRandom(new SeedR());
        }
        public SlotR(long seed)
        {
            rand = new RandomLib.NetRandom(new SeedR(seed));
        }

        public SlotR(RandEXom.Interface.IRandomR framework)
        {
            rand = framework;
        }

        public virtual int Next(int min, int max)
        {

            byte[] bMin = new byte[max.ToString().Length];
            byte[] bMax = new byte[max.ToString().Length];
            byte[] val = new byte[max.ToString().Length];

            //convert max value to array
            for(int i = 0; i < bMax.Length; i++)
            {
                bMax[i] = byte.Parse(max.ToString()[i].ToString());
            }

            //convert min value to array
            for (int i = 0; i < bMin.Length; i++)
            {

                //I dunno why cannot I use
                //bMin[i] = (i < bMin.Length - min.ToString().Length) ? 0 : byte.Parse(min.ToString()[i].ToString());
                // instead

                if (i < bMin.Length - min.ToString().Length) {
                    bMin[i] = 0;
                } 
                
                else
                {
                    //Console.Write("i = " + i + "; array length = " + bMin.Length + "min length = " + min.ToString().Length);
                    bMin[i] = byte.Parse(min.ToString()[bMin.Length - i -1].ToString());
                }
            }

            bool headIsSame = true;
            for (int i = 0; i < bMax.Length; i++)
            {
                if (headIsSame)
                {
                    if (bMax[i] != bMin[i])
                    {
                        headIsSame = false;
                        val[i] = (byte) rand.NextInt(bMin[i], bMax[i]);
                    }
                    else
                    {
                        val[i] = bMax[i];
                    } 
                }
                else
                {
                    val[i] = (byte)rand.NextInt(0, 10);
                }
            }

            string valStr = string.Empty;
            for (int i = 0; i < val.Length; i++)
            {
                valStr += val[i].ToString();
            }
            return int.Parse(valStr);
        }

    }
}
