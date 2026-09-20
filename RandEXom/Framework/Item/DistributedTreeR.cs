// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom

using System;
using System.Collections.Generic;
using RandEXom.Interface;
using RandEXom.SeedLib;

namespace RandEXom.Framework.Item
{
    /// <summary>
    /// Random item picker using endpoint groups equally divided at initialization.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DistributedTreeR<T>
    {
        readonly IRandomR rand;
        readonly int step;
        readonly int child;
        readonly bool remove_on_pull;
        readonly Node top;
        readonly List<Node> bottomNode = new List<Node>();
        int count;

        private class Node
        {
            public Node parent;
            public List<Node> childs = new List<Node>();
            public List<T> items = new List<T>();
        }

        public DistributedTreeR(IEnumerable<T> items, int step = 1, int child = 2, bool do_shuffle = false, bool remove_on_pull = false)
            : this(items, new RandomLib.NetRandom(new SeedR()), step, child, do_shuffle, remove_on_pull)
        {
        }

        public DistributedTreeR(IEnumerable<T> items, long seed, int step = 1, int child = 2, bool do_shuffle = false, bool remove_on_pull = false)
            : this(items, new RandomLib.NetRandom(new SeedR(seed)), step, child, do_shuffle, remove_on_pull)
        {
        }

        /// <summary>
        /// Divide items in index order, or shuffle before dividing.
        /// </summary>
        /// <param name="items">Items to copy into the tree.</param>
        /// <param name="framework">Random source using its own seed generator.</param>
        /// <param name="step">Number of branch choices. Zero uses one endpoint.</param>
        /// <param name="child">Number of branches for each node.</param>
        /// <param name="do_shuffle">Randomize item placement when creating the tree.</param>
        /// <param name="remove_on_pull">Remove selected items and lock empty endpoints.</param>
        public DistributedTreeR(IEnumerable<T> items, IRandomR framework, int step = 1, int child = 2, bool do_shuffle = false, bool remove_on_pull = false)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            rand = framework ?? throw new ArgumentNullException(nameof(framework));
            if (step < 0) throw new ArgumentOutOfRangeException(nameof(step));
            if (child < 1) throw new ArgumentOutOfRangeException(nameof(child));
            List<T> items_current = new List<T>(items);
            if (items_current.Count == 0) throw new ArgumentException("Items cannot be empty.", nameof(items));
            this.step = step;
            this.child = child;
            this.remove_on_pull = remove_on_pull;
            count = items_current.Count;
            int endpointCount = 1;
            if (child > 1)
            {
                for (int i = 0; i < step; i++)
                {
                    //check before multiplying so endpoint count cannot overflow
                    if (endpointCount > count / child)
                        throw new ArgumentOutOfRangeException(nameof(step), "More endpoints than items.");
                    endpointCount *= child;
                }
            }
            if (do_shuffle)
                new Shuffles_FisherYates(rand).Shuffle(items_current);

            top = CreateNodes(null, child == 1 ? 0 : step);
            int size = count / endpointCount;
            int remainder = count % endpointCount;
            int start = 0;
            for (int i = 0; i < endpointCount; i++)
            {
                //divide only once so pulling does not move items between endpoints
                int length = size + (i < remainder ? 1 : 0);
                bottomNode[i].items.AddRange(items_current.GetRange(start, length));
                start += length;
            }
        }

        private Node CreateNodes(Node parent, int remainingStep)
        {
            Node node = new Node { parent = parent };
            if (remainingStep == 0)
                bottomNode.Add(node);
            else
                for (int i = 0; i < child; i++)
                    node.childs.Add(CreateNodes(node, remainingStep - 1));
            return node;
        }

        /// <summary>
        /// Pick an available branch at each step, then keep or remove the selected item.
        /// Returns default(T) when all items have been pulled out.
        /// </summary>
        public virtual T Pull()
        {
            if (count == 0) return default(T);
            Node node = top;
            //single branch trees need no intermediate nodes, but still draw at each step
            if (child == 1)
                for (int i = 0; i < step; i++)
                    rand.NextInt(0, 1);
            while (node.childs.Count > 0)
                node = node.childs[rand.NextInt(0, node.childs.Count)];

            int index = rand.NextInt(0, node.items.Count);
            T item = node.items[index];
            if (remove_on_pull)
            {
                node.items.RemoveAt(index);
                count--;
                //lock empty endpoints and remove branches with no available endpoints
                while (node.items.Count == 0 && node.childs.Count == 0 && node.parent != null)
                {
                    Node parent = node.parent;
                    parent.childs.Remove(node);
                    node = parent;
                }
            }
            return item;
        }

        public List<T> ToList()
        {
            List<T> items = new List<T>(count);
            foreach (Node node in bottomNode)
                items.AddRange(node.items);
            return items;
        }

        public int Count()
        {
            return count;
        }
    }
}
