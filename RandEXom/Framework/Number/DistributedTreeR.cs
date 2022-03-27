// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom


using RandEXom.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.Framework.Number
{
    public class DistributedTreeR
    {
        readonly int level = 5;
        readonly int child = 2;
        readonly long min = int.MinValue;
        readonly ulong range = int.MaxValue - int.MaxValue;
        IRandomR random;

        public class Node
        {
            public static List<Node> nodes = new List<Node>();
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
                List<Node> childs = GetChilds(); 
                if (!childs.Contains(node))
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
            this.min = min;
            CreateNodes(level,child);
            this.range = unchecked((ulong)(max - min)) / (ulong)bottomNode.Count;            
        }

        public DistributedTreeR(long seed, int level = 5, int child =2, long min = int.MinValue, long max = int.MaxValue)
        {
            random = new RandomLib.NetRandom(seed);
            this.level = level;
            this.child = child;
            this.min = min;
            CreateNodes(level, child);
            this.range = unchecked((ulong)(max - min)) / (ulong)bottomNode.Count;
        }

        public DistributedTreeR(IRandomR random, int level = 5, int child = 2, long min = int.MinValue, long max = int.MaxValue)
        {
            this.random = random;
            this.level = level;
            this.child = child;
            this.min = min;
            CreateNodes(level, child);
            this.range = unchecked((ulong)(max - min)) / (ulong)bottomNode.Count;
        }

        public void CreateNodes(int level, int child)
        {
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
                        ch.parentID = top.ID;
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
            return (int)Next();
        }

        public virtual long Next()
        {
            List<Node> childs = top.GetChilds();

            if (bottomNode.Count() == 0)
                return random.NextLong(min, unchecked(min + (long)range));
            
            if (childs.Count == 0)
                return random.NextLong(min, unchecked( min + (long) range) * bottomNode.Count());

            childs = childs.OrderBy(x => x.weight).ToList();
            int minWeight = childs[0].weight;
            childs.RemoveAll(x => x.weight > minWeight);

            Node node = null;
            int i = 0;
            while (childs.Count > 0)
            {
                i = random.NextInt(0, childs.Count());
                node = childs[i];
                childs = node.GetChilds();
            }
            i = bottomNode.IndexOf(node);
            node.UpdateWeight(node.weight + 1);
            return random.NextLong(unchecked(min + (long)range) * (i), unchecked(min + (long)range) * (i + 1));
        }
    }
}