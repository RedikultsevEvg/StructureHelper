using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangulationOptions
    {
        LimitStates LimiteState { get; }
        CalcTerms CalcTerm { get; }
    }
}
