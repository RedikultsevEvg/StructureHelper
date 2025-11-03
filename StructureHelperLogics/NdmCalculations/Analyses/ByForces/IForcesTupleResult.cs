using LoaderCalculator.Data.ResultData;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForcesTupleResult : IResult
    {
        IForceTupleInputData InputData { get; set; }
        IDesignForceTuple DesignForceTuple { get; set; }
        ILoaderResults LoaderResults { get; set; }
    }
}