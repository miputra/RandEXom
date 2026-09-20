
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandEXom;
using RandEXom.RandomLib;
using RandEXom.Framework;
using RandEXom.Framework.Item;
using RandEXom.Framework.Number;

namespace RandExOmTestOut
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
                Console.WriteLine("[5] to test SSRNG");
                Console.WriteLine("[6] to test XORShift");
                Console.WriteLine("[7] to test XORShiftStar");
                Console.WriteLine("[8] to test SlotR");
                Console.WriteLine("[9] to test Distributed Tree Item");
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
                    case "5":
                        TestSSRNG();
                        break;
                    case "6":
                        TestXORShift();
                        break;
                    case "7":
                        TestXORShiftStar();
                        break;
                    case "8":
                        TestSlotNumber();
                        break;
                    case "9":
                        TestDistributedItem();
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
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(pong.Next(0, 1000));
            }
            Console.WriteLine("=====================================");
            Console.WriteLine();
            Console.WriteLine("Long Pong");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(pong.Next((long)0, (long)1000));
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

        static void TestSSRNG()
        {
            SSRNGRandom rand = new SSRNGRandom();

            Console.WriteLine("Test Int");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(rand.NextInt(0, 100));
            }

            Console.WriteLine("Test long");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(rand.NextLong(0, 10000));
            }
        }
        static void TestXORShift()
        {
            ModuloRandom rand = new ModuloRandom();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(rand.NextInt(0, 100));
            }

            Console.WriteLine("Test long");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(rand.NextLong(0, 10000));
            }
        }

        static void TestXORShiftStar()
        {
            //XORShift64StarRandom rand = new XORShift64StarRandom();
            ModuloRandom rand = new ModuloRandom(ModuloRandom.Multiplier.XORShift64_Star);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(rand.NextInt(0, 100));
            }

            Console.WriteLine("Test long");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(rand.NextLong(0, 10000));
            }
        }

        static void TestDistributedItem()
        {
            DateTime lastTime = DateTime.Now;
            List<string> items = Enumerable.Range(1, 12).Select(x => "Item" + x).ToList();

            foreach (bool do_shuffle in new[] { false, true })
            {
                foreach (bool remove_on_pull in new[] { false, true })
                {
                    ModuloRandom random = new ModuloRandom(new RandEXom.SeedLib.XORShift64Seed(123));
                    DistributedTreeR<string> rand = new DistributedTreeR<string>(items, random,
                        step: 2, child: 2, do_shuffle: do_shuffle, remove_on_pull: remove_on_pull);
                    Console.WriteLine("=====================================");
                    Console.WriteLine("Shuffle: " + do_shuffle + ", Remove on pull: " + remove_on_pull);

                    //save initial groups so we can see which endpoints become locked
                    List<string> initial = rand.ToList();
                    for (int endpoint = 0; endpoint < 4; endpoint++)
                        Console.WriteLine("Endpoint " + endpoint + ": " + string.Join(", ", initial.Skip(endpoint * 3).Take(3)));

                    HashSet<string> pulled = new HashSet<string>();
                    for (int i = 0; i < items.Count; i++)
                    {
                        string get = rand.Pull();
                        int endpoint = initial.IndexOf(get) / 3;
                        Console.WriteLine(get + " from endpoint " + endpoint + ", Remaining: " + rand.Count());
                        if (remove_on_pull)
                        {
                            if (!pulled.Add(get)) throw new Exception("Item pulled more than once.");
                            if (rand.Count() != items.Count - i - 1) throw new Exception("Wrong remaining item count.");
                            if (!rand.ToList().Intersect(initial.Skip(endpoint * 3).Take(3)).Any())
                                Console.WriteLine("Endpoint " + endpoint + " is empty and locked.");
                        }
                        else if (!rand.ToList().SequenceEqual(initial))
                            throw new Exception("Items changed when keeping the pulled item.");
                    }

                    if (remove_on_pull)
                    {
                        if (!pulled.SetEquals(items) || rand.ToList().Count != 0 || rand.Pull() != null)
                            throw new Exception("Tree did not finish empty.");
                        Console.WriteLine("All endpoints locked. Empty pull returns null.");
                    }
                    Console.WriteLine("Test passed.");
                }
            }
            string finish = (DateTime.Now - lastTime).TotalMilliseconds.ToString();
            Console.WriteLine("Process is done by " + finish + "ms");
        }

        static void TestSlotNumber()
        {
            SlotR rand = new SlotR();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(rand.Next(0, 100));
            }



        }


    }
}
