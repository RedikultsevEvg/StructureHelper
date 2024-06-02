using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.RC
{
    public interface IAnchorageCalculator
    {
        double GetBaseDevLength();
        double GetDevLength();
        double GetLapLength();
    }
}
