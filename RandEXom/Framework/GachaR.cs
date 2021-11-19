// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom

using System.Collections.Generic;
using System.Linq;
using RandEXom.SeedLib;

namespace RandEXom.Framework
{
    /// <summary>
    /// Gacha or random picker framework. Heavy on memory because saving the entire item but low in process
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GachaR<T> : RandEXom.Interface.IGachaR<T>
    {
        List<T> items_init = new List<T>();
        List<T> items_current = new List<T>();

        RandEXom.Interface.IRandomR rand;

        public GachaR()
        {
            rand = new RandomLib.NetRandom(new SeedR());
        }
        public GachaR(long seed)
        {
            rand = new RandomLib.NetRandom(new SeedR(seed));
        }

        public GachaR(RandEXom.Interface.IRandomR framework)
        {
            rand = framework;
        }

        public virtual void AddItem(T item, int count)
        {
            for(int i=0; i < count; i++)
            {
                items_init.Add(item);
                items_current.Add(item);
            }
        }

        public virtual void RemoveEmpty(T item)
        {
            items_init.Clear();
            items_init.AddRange(items_current);            
        }

        public virtual void Refill()
        {
            items_current.Clear();
            items_current.AddRange(items_init);
        }

        public virtual void Remove(T value)
        {
            items_init.Remove(value);
            items_current.Remove(value);
        }

        public virtual T Pull()
        {
            if (items_current.Count <= 0)
                return default(T);
            int n = rand.NextInt(0, items_current.Count);
            T get = items_current[n];
            items_current.RemoveAt(n);
            return get;
        }

        /// <inheritdoc/>
        public int Count()
        {
            return items_current.Count();
        }
        /// <inheritdoc/>
        public int Count(T type)
        {
            dynamic t1 = type;
            return items_current.FindAll(x => x == t1).Count();
        }
        /// <inheritdoc/>
        public int CountType()
        {
            HashSet<T> set = new HashSet<T>(items_current);
            return set.Count;
        }
    }
}
