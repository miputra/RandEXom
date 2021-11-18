using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.SeedLib
{
    /// <summary>
    /// Default seed. The value will not changed every random process.
    /// </summary>
    class Seed : RandEXom.Interface.ISeedR
    {
        private long _seed = 0;
        public long init
        {
            get
            {
                return _seed;
            }
        }

        public long previous
        {
            get
            {
                return _seed;
            }
        }

        public long now
        {
            get
            {
                return _seed;
            }
        }

        public Seed(long? seed = null)
        {
            long new_seed = 0;
            if (seed == null)
                new_seed = long.Parse(
                    System.DateTime.Now.Millisecond.ToString() +
                    System.DateTime.Now.Second.ToString() +
                    System.DateTime.Now.Minute.ToString() +
                    System.DateTime.Now.Hour.ToString() +
                    System.DateTime.Now.Day.ToString() +
                    System.DateTime.Now.Month.ToString() +
                    System.DateTime.Now.Year.ToString().Substring(2, 2)

                    );
            else
                new_seed = (long) seed;
            this._seed = new_seed;
        }

        public virtual void Next()
        {
        }


        
    }
}
