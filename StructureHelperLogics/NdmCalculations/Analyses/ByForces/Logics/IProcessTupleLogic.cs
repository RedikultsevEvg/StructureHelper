using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IProcessTupleLogic : ILogic
    {
        IForceTupleInputData InputData { get; set; }
        IForcesTupleResult ProcessNdmResult();
    }
}
