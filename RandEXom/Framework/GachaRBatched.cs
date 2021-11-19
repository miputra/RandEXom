// Made by Muhammad Ihsan Diputra
// Lincense under MIT
// https://github.com/miputra/RandEXom

using System;
using System.Collections.Generic;
using System.Linq;
using RandEXom.SeedLib;

namespace RandEXom.Framework
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
                c += p.Value;
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
            if (!items_init.ContainsKey(item))
            {
                items_init.Add(item, 0);
                items_current.Add(item, 0);
            }
            items_init[item] += count;
            items_current[item] += count;
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

            int n = Count();
            int r = rand.NextInt(0, n);
            Dictionary<T, double> temp = new Dictionary<T, double>();

            double minVal = 0;
            foreach (KeyValuePair<T,int> pair in items_current)
            {
                double p = (double)pair.Value / (double)n;
                if (temp.Count <= 0)
                    minVal = p;
                else
                {
                    minVal = Math.Min(p, minVal);
                }
                temp.Add(pair.Key, p);
            }
            double maxVal = 0;
            while (minVal < 2)
            {
                maxVal = 0;
                for(int i=0; i < temp.Keys.Count; i++)
                {
                    T key = temp.Keys.ToList()[i];
                    temp[key] *= 10;
                    if (i == 0) {
                        minVal = temp[key];
                    } else
                    {
                        minVal = Math.Min(temp[key], minVal);
                    }
                    maxVal = Math.Max(maxVal, temp[key]);
                }
            }

            List<T> o = temp.Keys.OrderBy(x => temp[x]).ToList();
            foreach (T get in o)
            {               

                if (r % Math.Round(maxVal - temp[get]) == 0)
                {
                    items_current[get] -= 1;
                    if (items_current[get] <= 0)
                        items_current.Remove(get);
                    return get;
                }
            }

            if(o.Count > 0)
            {
                T get = o[o.Count - 1];
                items_current[get] -= 1;
                if (items_current[get] <= 0)
                    items_current.Remove(get);
                return get;
            }

            return default(T);
  
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