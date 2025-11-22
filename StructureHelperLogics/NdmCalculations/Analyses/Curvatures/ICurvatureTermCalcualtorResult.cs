using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureTermCalcualtorResult : IResult
    {
        ICurvatureTermCalcualtorInputData InputData { get; set; }
        IForceTuple CurvatureValue { get; set; }
        IForceTuple Deflection { get; set; }

    }
}
