using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.Interface
{
    public interface IRandom
    {
        string GetSeed();
        int NextInt(int min, int max);
        void NextBytes(byte[] buffers);
        long NextLong(long min, long max);

    }
}
