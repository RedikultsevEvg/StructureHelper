namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorResult : ICurvatureCalculatorResult
    {
        public ICurvatureCalculatorInputData InputData { get; set; }

        public List<ICurvatureForceCalculatorResult> ForceCalculatorResults { get; } = [];

        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}
