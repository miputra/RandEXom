using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.Utility
{
    internal class SeedGenerator
    {
        public static long GetJoinedCurrentDate()
        {
            return long.Parse(
                    System.DateTime.Now.Millisecond.ToString() +
                    System.DateTime.Now.Second.ToString() +
                    System.DateTime.Now.Minute.ToString() +
                    System.DateTime.Now.Hour.ToString() +
                    System.DateTime.Now.Day.ToString() +
                    System.DateTime.Now.Month.ToString() +
                    System.DateTime.Now.Year.ToString().Substring(2, 2)

                    );
        }
    }
}
