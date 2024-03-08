using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal class CriticalForceSP63Logic : ICriticalBucklingForceLogic
    {
        double concreteFactor, reinforcementFactor;

        public IShiftTraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double GetCriticalForce()
        {
            throw new NotImplementedException();
        }

        public double GetEtaFactor()
        {
            throw new NotImplementedException();
        }
    }
}
