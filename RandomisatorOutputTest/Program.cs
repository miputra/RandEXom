using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandEXom;
using RandEXom.RandomLib;
using RandEXom.Framework;

namespace RandomisatorOutputTest
{
    class Program
    {
        static void Main(string[] args)
        {          

            while (true)
            {
                Console.WriteLine("[1] to test gacha");
                Console.WriteLine("[2] to test gachalite");
                string res = Console.ReadLine();
                if (res == "1")
                    TestGacha();
                if (res == "2")
                    TestGachaLite();
                if (res == string.Empty)
                    return;
            }          

        }

        static void TestGacha()
        {
            DateTime lastTime = DateTime.Now;
            Gacha<string> rarity = new Gacha<string>();
            rarity.AddItem("Common1", 1000);
            rarity.AddItem("Common2", 1000);
            rarity.AddItem("Rare1", 500);
            rarity.AddItem("Rare2", 500);
            rarity.AddItem("SR1", 100);
            rarity.AddItem("SR2", 100);
            rarity.AddItem("Ultimate", 10);

            for (int i = 0; i < 1000; i++)
            {
                string get = rarity.Pull();
                if (get != null)
                {
                    Console.WriteLine(get);
                }
            }
            string finish = (DateTime.Now - lastTime).Milliseconds.ToString();

            Console.WriteLine("Process is done by " + finish + "ms");
        }
    
        static void TestGachaLite()
        {
            DateTime lastTime = DateTime.Now;
            GachaBatched<string> rarity = new GachaBatched<string>();
            rarity.AddItem("Common1", 1000);
            rarity.AddItem("Common2", 1000);
            rarity.AddItem("Rare1", 500);
            rarity.AddItem("Rare2", 500);
            rarity.AddItem("SR1", 100);
            rarity.AddItem("SR2", 100);
            rarity.AddItem("Ultimate", 10);

            for (int i = 0; i < 1000; i++)
            {
                string get = rarity.Pull();
                if (get != null)
                {
                    Console.WriteLine(get);
                }
            }
            string finish = (DateTime.Now - lastTime).Milliseconds.ToString();

            Console.WriteLine("Process is done by " + finish + "ms");
        }
    }
}