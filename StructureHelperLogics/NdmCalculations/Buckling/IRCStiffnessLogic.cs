using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public interface IRCStiffnessLogic : ILogic
    {
        (double Kc, double Ks) GetStiffnessCoeffitients();
    }
}
