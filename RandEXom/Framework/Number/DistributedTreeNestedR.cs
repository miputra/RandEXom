

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
    [ObsoleteAttribute("This class still in development. Expect for some unexpected result. Use DistributedTreeR instead", false)]
    public class DistributedTreeNestedR
    {
        readonly int level = 5;
        readonly int child = 2;
        readonly long min = int.MinValue;
        readonly ulong range = int.MaxValue - int.MaxValue;
        IRandomR random;

        public class Node
        {
            public Node parent;
            public List<Node> childs = new List<Node>();

            //this apperently not good if there is a seriliazition limit.
            //public List<Node> child = new List<Node>();
            private int _weight = 0;
            public int weight { get { return _weight; } }

            public int _prevWeight = 0;
            public int prevWeight { get { return _prevWeight; } }

            public Node()
            {
            }

            public Node(Node parent)
            {
                this.parent = parent;
            }

            public void UpdateWeight(int weight)
            {
                this._prevWeight = this._weight;
                this._weight = weight;
                UpdateWeightFB(this);
            }


            /// <summary>
            /// update weight from bottom node
            /// </summary>
            public void UpdateWeightFB(Node node, bool updateWeight = false)
            {
                if (!childs.Contains(node))
                {
                    //do this so we doesn't need to recount all of nodes
                    if (updateWeight)
                    {
                        int weight = (this.weight - node.prevWeight) + node.weight;
                        this._prevWeight = this._weight;
                        this._weight = weight;
                    }

                    if (parent != null)
                    {
                        parent.UpdateWeightFB(this, true); //.UpdateWeight(weight);
                    }
                }
                return;
            }
        }

        private Dictionary<int, int> nodeCounter = new Dictionary<int, int>();

        Node top;
        List<Node> bottomNode = new List<Node>();

        public DistributedTreeNestedR(int level = 5, int child = 2, long min = int.MinValue, long max = int.MaxValue)
        {
            random = new RandomLib.NetRandom();
            this.level = level;
            this.child = child;
            this.min = min;
            CreateNodes(level, child);
            this.range = unchecked((ulong)(max - min)) / (ulong)bottomNode.Count;
        }

        public DistributedTreeNestedR(long seed, int level = 5, int child = 2, long min = int.MinValue, long max = int.MaxValue)
        {
            random = new RandomLib.NetRandom(seed);
            this.level = level;
            this.child = child;
            this.min = min;
            CreateNodes(level, child);
            this.range = unchecked((ulong)(max - min)) / (ulong)bottomNode.Count;
        }

        public DistributedTreeNestedR(IRandomR random, int level = 5, int child = 2, long min = int.MinValue, long max = int.MaxValue)
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
            List<Node> parents = new List<Node> { top };
            List<Node> childrens = new List<Node>();

            for (int lv = 1; lv < level; lv++)
            {
                foreach (Node parent in parents)
                {
                    for (int i = 0; i < child; i++)
                    {
                        Node ch = new Node();
                        ch.parent = top;
                        childrens.Add(ch);
                        parent.childs.Add(ch);
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
            List<Node> childs = top.childs;

            if (bottomNode.Count() == 0)
                return random.NextLong(min, unchecked(min + (long)range));

            if (childs.Count == 0)
                return random.NextLong(min, unchecked(min + (long)range) * bottomNode.Count());

            childs = childs.OrderBy(x => x.weight).ToList();
            int minWeight = childs[0].weight;
            childs.RemoveAll(x => x.weight > minWeight);

            Node node = null;
            int i = 0;
            while (childs.Count > 0)
            {
                i = random.NextInt(0, childs.Count());
                node = childs[i];
                childs = node.childs;
            }
            i = bottomNode.IndexOf(node);
            node.UpdateWeight(node.weight + 1);
            return random.NextLong(unchecked(min + (long)range) * (i), unchecked(min + (long)range) * (i + 1));
        }
    }
}
