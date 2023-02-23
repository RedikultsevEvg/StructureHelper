using LoaderCalculator.Data.ResultData;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForcesTupleResult : INdmResult
    {
        IDesignForceTuple DesignForceTuple { get; set; }
        ILoaderResults LoaderResults { get; set; }
    }
}