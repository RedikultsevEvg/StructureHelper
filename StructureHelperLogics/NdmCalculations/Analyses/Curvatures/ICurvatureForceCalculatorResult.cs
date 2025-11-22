using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureForceCalculatorResult : IResult
    {
        ICurvatureForceCalculatorInputData InputData { get; set; }
        ICurvatureTermCalcualtorResult LongTermResult { get; set; }
        ICurvatureTermCalcualtorResult ShortTermResult { get; set; }
    }
}
