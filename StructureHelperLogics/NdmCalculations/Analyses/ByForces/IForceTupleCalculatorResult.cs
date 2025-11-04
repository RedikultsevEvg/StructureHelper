using LoaderCalculator.Data.ResultData;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceTupleCalculatorResult : IResult
    {
        IForceTupleInputData InputData { get; set; }
        IForceTuple ForceTuple { get; set; }
        ILoaderResults LoaderResults { get; set; }
    }
}