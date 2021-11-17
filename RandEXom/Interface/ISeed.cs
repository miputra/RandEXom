using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandEXom.Interface
{
    
    public interface ISeed
    {
        long init { get;}
        long now { get;}
        long previous { get; }
        void Next();
    }
}
