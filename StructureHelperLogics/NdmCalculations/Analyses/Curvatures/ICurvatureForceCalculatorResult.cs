using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureForceCalculatorResult : IResult
    {
        ICurvatureForceCalculatorInputData InputData { get; set; }
        ICurvatureSectionResult SectionResult { get; set; }
    }
}
