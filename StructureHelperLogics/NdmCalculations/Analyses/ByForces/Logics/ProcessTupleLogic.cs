using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ProcessTupleLogic : IProcessTupleLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }
        IForceTupleInputData IProcessTupleLogic.InputData { get; set; }

        public IForcesTupleResult ProcessNdmResult()
        {
            throw new NotImplementedException();
        }
    }
}
