using RandEXom.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.Framework.Number
{
    public class DistributedTree
    {
        readonly int level = 5;
        readonly int child = 2;
        readonly long min = int.MinValue;
        readonly ulong range = int.MaxValue - int.MaxValue;
        IRandomR random;

        public class Node
        {
            public Node parent;
            public List<Node> child = new List<Node>();
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
            }



            /// <summary>
            /// update weight from node
            /// </summary>
            public void UpdateWeightFB(Node node)
            {
                if (!child.Contains(node))
                {
                    //do this so we doesn't need to recount all of nodes
                    int weight = (this.weight - node.prevWeight) + node.weight;
                    UpdateWeight(weight);
                    if(parent != null)
                    {
                        parent.UpdateWeight(weight);
                    }
                }
                return;
            }
        }

        private Dictionary<int, int> nodeCounter = new Dictionary<int, int>();

        Node top;
        List<Node> bottomNode = new List<Node>();

        public DistributedTree(int level = 5,int child = 2, long min = int.MinValue, long max = int.MaxValue)
        {
            random = new RandomLib.NetRandom();
            this.level = level;
            this.child = child;
            this.min = min;
            top = CreateNode();
            this.range = unchecked((ulong)(max - min)) / (ulong)bottomNode.Count;            
        }

        public DistributedTree(long seed, int level = 5, int child =2, long min = int.MinValue, long max = int.MaxValue)
        {
            random = new RandomLib.NetRandom(seed);
            this.level = level;
            this.child = child;
            this.min = min;
            top = CreateNode();
            this.range = unchecked((ulong)(max - min)) / (ulong)bottomNode.Count;
        }

        public DistributedTree(IRandomR random, int level = 5, int child = 2, long min = int.MinValue, long max = int.MaxValue)
        {
            this.random = random;
            this.level = level;
            this.child = child;
            this.min = min;
            top = CreateNode();
            this.range = unchecked((ulong)(max - min)) / (ulong)bottomNode.Count;
        }

        public void CreateNodes(int level, int child)
        {

        }

        

        //protected virtual int GetMinNode(int level)
        //{
            
        //}


        /// recursive is very bad in performance, and not all builder support deep recursive,
        //public virtual Node CreateNode(Node node = null, int i=0)
        //{

            

        //    Node _node = new Node(node);
        //    if (i < level)
        //    {
        //        for (int j = 0; j < child; j++)
        //        {
        //            _node.child.Add(CreateNodeRecursive(_node, i + 1));
        //        }
        //    }
        //    else
        //    {
        //        bottomNode.Add(_node);
        //    }
        //    return _node;
        //}

        public virtual Node CreateNode()
        {
            Node node_0 = new Node(null);
            List<Node> currentNode = new List<Node>();
            List<Node> childNode = new List<Node>();
            currentNode.Add(node_0);
            for(int lv = 1; lv <= level; lv++)
            {
                int pi = 0;
                Node p = currentNode[pi];
                for(int i=1; i <= Math.Pow(child,lv); i++)
                {
                    p = currentNode[pi];
                    p.child.Add(p);
                    childNode.Add(p);
                    if (i % child == 0)
                        pi++;                    
                }
                currentNode.Clear();
                currentNode.AddRange(childNode);
                childNode.Clear();
            }
            this.bottomNode = currentNode;

            return node_0;
        }
    }
}