using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal interface ICriticalBucklingForceLogic : ILogic
    {
        double GetCriticalForce();
        double GetEtaFactor();
    }
}
