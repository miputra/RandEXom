// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom

using System;
using System.Collections.Generic;
using System.Linq;
using RandEXom.SeedLib;

namespace RandEXom.Framework.Item
{
    /// <summary>
    /// Gacha or random picker framework. Heavy on process, but low on memory because only using sample
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GachaRBatched<T> : RandEXom.Interface.IGachaR<T>
    {        
        Dictionary<T, int> items_init = new Dictionary<T, int>();
        Dictionary<T, int> items_current = new Dictionary<T, int>();

        RandEXom.Interface.IRandomR rand;

        /// <inheritdoc/>
        public int Count()
        {
            int c = 0;
            foreach(KeyValuePair<T,int> p in items_current)
            {
                c = checked(c + p.Value);
            }
            return c;
        }

        /// <inheritdoc/>
        public int Count(T type)
        {
            if (items_current.ContainsKey(type))
                return items_current[type];
            return 0;
        }

        /// <inheritdoc/>
        public int CountType()
        {
            return items_current.Count;
        }

        public GachaRBatched(long seed)
        {
            rand = new RandomLib.NetRandom(new SeedR(seed));
        }

        public GachaRBatched()
        {
            rand = new RandomLib.NetRandom(new SeedR());
        }

        public GachaRBatched(RandEXom.Interface.IRandomR framework)
        {
            rand = framework;
        }

        public void AddItem(T item, int count)
        {
            //if(items_init.Find(x=> x.value == item))
            //items_init.Add(new Item<T>(item,count));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (!items_init.ContainsKey(item)) items_init.Add(item, 0);
            if (!items_current.ContainsKey(item)) items_current.Add(item, 0);
            items_init[item] = checked(items_init[item] + count);
            items_current[item] = checked(items_current[item] + count);
        }

        /// <inheritdoc/>
        public void RemoveEmpty(T item, bool isIterative = false)
        {
            switch (isIterative)
            {
                case false: //Make a new directory instead. Heavy in memory size, and will create a dump
                    items_init = new Dictionary<T, int>(items_current);
                    break;
                case true: //Iterative. Heavy on process, but will not create a dump
                    items_init.Clear();
                    foreach (KeyValuePair<T, int> pair in items_current)
                    {
                        items_init.Add(pair.Key, pair.Value);
                    }
                    break;
            }
        }
        /// <inheritdoc/>
        public void Refill(bool isIterative = false)
        {
            switch (isIterative)
            {
                case false: //Make a new directory instead. Heavy in memory size, and will create a dump
                    items_current = new Dictionary<T, int>(items_init);
                    break;
                case true: //Iterative. Heavy on process, but will not create a dump
                    items_current.Clear();
                    foreach (KeyValuePair<T, int> pair in items_init)
                    {
                        items_current.Add(pair.Key, pair.Value);
                    }
                    break;
            }
        }

        public void Remove(T value)
        {
            items_init.Remove(value);
            items_current.Remove(value);
        }

        public T Pull()
        {
            if (items_current.Keys.Count <= 0)
                return default(T);

            int remaining = Count();
            if (remaining <= 0) return default(T);
            int draw = rand.NextInt(0, remaining);
            foreach (KeyValuePair<T, int> pair in items_current.ToList())
            {
                if (draw < pair.Value)
                {
                    items_current[pair.Key]--;
                    if (items_current[pair.Key] == 0) items_current.Remove(pair.Key);
                    return pair.Key;
                }
                draw -= pair.Value;
            }
            throw new InvalidOperationException("Item counts are inconsistent.");
        }

        public void RemoveEmpty(T item)
        {
            RemoveEmpty(item,false);
        }

        public void Refill()
        {
            Refill(false);
        }
    }    
}