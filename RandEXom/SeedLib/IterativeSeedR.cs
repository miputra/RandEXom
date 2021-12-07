using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandEXom.Interface;

namespace RandEXom.SeedLib
{

    /// <summary>
    /// The value will be change and iterate every random process
    /// </summary>
    public class IterativeSeedR : ISeedR
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
        public enum Process
        {
            Addition,
            Substraction,
            Multiplication,
            Cos,
            Sin
        }
        public Process process = Process.Addition;
        
        public IterativeSeedR(long ? seed = null)
        {
            long new_seed = 0;
            if (seed == null)
                this._seed = long.Parse(
                    System.DateTime.Now.Millisecond.ToString() +
                    System.DateTime.Now.Second.ToString() +
                    System.DateTime.Now.Minute.ToString() +
                    System.DateTime.Now.Hour.ToString() +
                    System.DateTime.Now.Day.ToString() +
                    System.DateTime.Now.Month.ToString() +
                    System.DateTime.Now.Year.ToString().Substring(2,2)          
                    
                    );
            else
                this._seed = (long)seed;
            Console.WriteLine("init seed = " + new_seed);
            Console.WriteLine("init seed = " + this.init);
            this.currentSeed = _seed;
            this.currentSeed = currentSeed == 0 ? currentSeed + 1 : currentSeed;
            this.previousSeed = currentSeed;
        }

        /// <summary>
        /// Generate next seed
        /// </summary>
        public virtual void Next()
        {
            this.previousSeed = currentSeed;
            switch (process)
            {
                case Process.Addition:
                    this.currentSeed += 1;
                    break;
                case Process.Substraction:
                    this.currentSeed -= 1;
                    break;
                case Process.Multiplication:
                    this.currentSeed *= 2;
                    break;
                case Process.Cos:
                    this.currentSeed = long.MaxValue - (long)(
                        (double) long.MaxValue * Math.Cos((double) RandEXom.Utility.TypeR.RoundLongToInt(this.currentSeed))
                        );
                    break;
                case Process.Sin:
                    this.currentSeed = this.currentSeed = long.MaxValue - (long)(
                        (double)long.MaxValue * Math.Sin((double)RandEXom.Utility.TypeR.RoundLongToInt(this.currentSeed))
                        );
                    break;
            }
            currentSeed = currentSeed == 0 ? currentSeed + 1 : currentSeed;
        }        

    }
}
