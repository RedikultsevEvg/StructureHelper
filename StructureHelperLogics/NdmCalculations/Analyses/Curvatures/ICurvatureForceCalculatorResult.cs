using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureForceCalculatorResult : IResult
    {
        ICurvatureForceCalculatorInputData InputData { get; set; }
        ICurvatureTermCalculatorResult LongTermResult { get; set; }
        ICurvatureTermCalculatorResult ShortTermResult { get; set; }
    }
}
