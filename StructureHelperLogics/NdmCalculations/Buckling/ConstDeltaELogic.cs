using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public class ConstDeltaELogic : IConcreteDeltaELogic
    {
        public double GetDeltaE()
        {
            return 1.5d;
        }
    }
}
