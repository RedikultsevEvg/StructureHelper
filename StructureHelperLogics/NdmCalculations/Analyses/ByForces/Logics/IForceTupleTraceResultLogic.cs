using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.ResultData;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceTupleTraceResultLogic : ILogic
    {
        void TraceResult(IResult result);
    }
}