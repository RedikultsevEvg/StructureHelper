using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal class ConstPhiLLogic : IConcretePhiLLogic
    {
        public double GetPhil()
        {
            return 2.0d;
        }
    }
}
