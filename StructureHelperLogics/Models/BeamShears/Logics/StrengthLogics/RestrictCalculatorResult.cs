using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    public class RestrictCalculatorResult : IResult
    {
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public IInclinedSection InclinedCrack { get; set; }
        public double StirrupStrength { get; set; }
    }
}
