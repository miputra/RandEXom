// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom


using RandEXom.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RandEXom.Framework.Number
{
    [EditorBrowsable(EditorBrowsableState.Never)]  // hidden from IntelliSense
    [Obsolete("There is an unexpected result to this class and need to be fixed", error: false)]      // warning if somehow used
    public class DistributedTreeR
    {
        readonly int level = 5;
        readonly int child = 2;
        readonly long min = int.MinValue;
        readonly long max = int.MaxValue;

        IRandomR random;

        public class Node
        {
            public static List<Node> nodes = new List<Node>(); //THIS THING SHOULD NOT STATIC. WAIT FOR FIX TO USE ENTIRE CLASS
            private static uint idlast = 0;
            public uint parentID;
            public uint ID;
            public List<uint> childs = new List<uint>();

            //this apperently not good if there is a seriliazition limit.
            //public List<Node> child = new List<Node>();
            private int _weight = 0;
            public int weight { get { return _weight; } }

            public int _prevWeight = 0;
            public int prevWeight { get { return _prevWeight; } }

            public Node()
            {
                parentID = 0;
                ID = ++idlast;
                idlast = ID;
                nodes.Add(this);
            }

            public Node(Node parent)
            {
                this.parentID = parent.ID;
                ID = ++idlast;
                idlast = ID;
                nodes.Add(this);
            }

            public void UpdateWeight(int weight)
            {
                this._prevWeight = this._weight;
                this._weight = weight;
                UpdateWeightFB(this);
            }

            public List<Node> GetChilds()
            {
                List<Node> list = new List<Node>();


                //may not efficient because searching for all nodes
                //Node child = nodes.FindAll(x => x.ID == this.parentID);
                for (int i = 0; i < this.childs.Count; i++)
                {
                    Node child = nodes.Find(x => x.ID == this.childs[i]);
                    list.Add(child);
                }
                return list;
            }

            /// <summary>
            /// update weight from bottom node
            /// </summary>
            public void UpdateWeightFB(Node node, bool updateWeight = false)
            {
                {
                    //do this so we doesn't need to recount all of nodes
                    if (updateWeight)
                    {
                        int weight = (this.weight - node.prevWeight) + node.weight;
                        this._prevWeight = this._weight;
                        this._weight = weight;
                    }

                    if (parentID != 0)
                    {
                        Node parent = nodes.Find(x=> x.ID == parentID);
                        parent.UpdateWeightFB(this, true); //.UpdateWeight(weight);
                    }
                }
                return;
            }
        }

        private Dictionary<int, int> nodeCounter = new Dictionary<int, int>();

        Node top;
        List<Node> bottomNode = new List<Node>();

        public DistributedTreeR(int level = 5,int child = 2, long min = int.MinValue, long max = int.MaxValue)
        {
            random = new RandomLib.NetRandom();
            this.level = level;
            this.child = child;
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            this.min = min;
            this.max = max;
            CreateNodes(level,child);
            if ((BigInteger)bottomNode.Count > (BigInteger)max - min)
                throw new ArgumentOutOfRangeException(nameof(level), "More leaves than values in the range.");
        }

        public DistributedTreeR(long seed, int level = 5, int child =2, long min = int.MinValue, long max = int.MaxValue)
        {
            random = new RandomLib.NetRandom(seed);
            this.level = level;
            this.child = child;
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            this.min = min;
            this.max = max;
            CreateNodes(level, child);
            if ((BigInteger)bottomNode.Count > (BigInteger)max - min)
                throw new ArgumentOutOfRangeException(nameof(level), "More leaves than values in the range.");
        }

        public DistributedTreeR(IRandomR random, int level = 5, int child = 2, long min = int.MinValue, long max = int.MaxValue)
        {
            this.random = random;
            this.level = level;
            this.child = child;
            if (min >= max) throw new ArgumentOutOfRangeException(nameof(max));
            this.min = min;
            this.max = max;
            CreateNodes(level, child);
            if ((BigInteger)bottomNode.Count > (BigInteger)max - min)
                throw new ArgumentOutOfRangeException(nameof(level), "More leaves than values in the range.");
        }

        public void CreateNodes(int level, int child)
        {
            if (level < 1) throw new ArgumentOutOfRangeException(nameof(level));
            if (child < 1) throw new ArgumentOutOfRangeException(nameof(child));
            top = new Node();
            List<Node> parents = new List<Node> {top};
            List<Node> childrens = new List<Node>();

            for (int lv=1; lv<level; lv++)
            {
                foreach (Node parent in parents)
                {
                    for (int i = 0; i < child; i++)
                    {
                        Node ch = new Node();
                        ch.parentID = parent.ID;
                        childrens.Add(ch);
                        parent.childs.Add(ch.ID);
                    }
                }
                parents.Clear();
                parents.AddRange(childrens);
                childrens.Clear();
            }
            bottomNode.Clear();
            bottomNode.AddRange(parents);
        }

        public virtual int NextInt()
        {
            return checked((int)Next());
        }

        public virtual long Next()
        {
            Node node = top;
            List<Node> children;
            while ((children = node.GetChilds()).Count > 0)
            {
                int least = children.Min(x => x.weight);
                List<Node> choices = children.Where(x => x.weight == least).ToList();
                node = choices[random.NextInt(0, choices.Count)];
            }
            int index = bottomNode.IndexOf(node);
            BigInteger span = (BigInteger)max - min;
            BigInteger start = (BigInteger)min + span * index / bottomNode.Count;
            BigInteger end = (BigInteger)min + span * (index + 1) / bottomNode.Count;
            if (start == end) throw new InvalidOperationException("More leaves than values in the range.");
            node.UpdateWeight(node.weight + 1);
            return random.NextLong((long)start, (long)end);
        }
    }
}
