using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IMaterialPartialFactor : IPartialFactor
    {
        StressStates StressState { get; set; }
        CalcTerms CalcTerm { get; set; }
        LimitStates LimitState { get; set; }
    }
}
