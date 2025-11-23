using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureTermCalculatorResult : IResult
    {
        ICurvatureTermCalculatorInputData InputData { get; set; }
        IForceTuple CurvatureValues { get; set; }
        IForceTuple Deflections { get; set; }

    }
}
