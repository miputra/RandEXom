using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.SeedLib
{
    /// <summary>
    /// If you want to custom how the seed work. Each of the random process will use a new seed by your function.
    /// </summary>
    class IterativeSeedCustom : RandEXom.Interface.ISeedR
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
                return previousSeed;
            }
        }

        public long now
        {
            get
            {
                return currentSeed;
            }
        }
        private long currentSeed = 0;
        private long previousSeed = 0;
        Func<long, long> process;

        /// <summary>
        /// Use your process here for generating next seed
        /// </summary>
        /// <param name="process">The input of process is previous seed, where the output is generated seed</param>
        /// <param name="seed"></param>
        public IterativeSeedCustom(Func<long, long> process, long? seed = null)
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
                new_seed = (long)seed;
            this._seed = new_seed;
            this.process = process;
        }

        public void Next()
        {
            this.previousSeed = currentSeed;

            this.currentSeed = process(currentSeed);
        }
    }
}
