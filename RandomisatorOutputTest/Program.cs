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
                Console.WriteLine("[3] to test pong");
                Console.WriteLine("[4] to test distance");
                string res = Console.ReadLine();
                switch (res)
                {
                    case "1":
                        TestGacha();
                        break;
                    case "2":
                        TestGachaLite();
                        break;
                    case "3":
                        TestPong();
                        break;
                    case "4":
                        TestDistance();
                        break;
                    default:
                        return;
                }

            }          

        }

        static void TestGacha()
        {
            DateTime lastTime = DateTime.Now;
            GachaR<string> rarity = new GachaR<string>();
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
            GachaRBatched<string> rarity = new GachaRBatched<string>();
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
    
        static void TestPong()
        {
            DateTime lastTime = DateTime.Now;
            PongR pong = new PongR();
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("Int Pong");
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine(pong.Next(0, 1000));
            }
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("Long Pong");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(pong.Next((long)0,(long) 1000));
            }


            string finish = (DateTime.Now - lastTime).Milliseconds.ToString();
            Console.WriteLine("Process is done by " + finish + "ms");
        }

        static void TestDistance()
        {
            DateTime lastTime = DateTime.Now;
            DistanceR distance = new DistanceR(100);
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("Int Distance");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(distance.Next(0, 1000));
            }
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("Long Distance");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(distance.Next((long)0, (long)1000));
            }

            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("Mix Distance");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(distance.Next(0, 1000));
                Console.WriteLine(distance.Next((long)0, (long)1000));
            }


            string finish = (DateTime.Now - lastTime).Milliseconds.ToString();
            Console.WriteLine("Process is done by " + finish + "ms");
        }
    }
}