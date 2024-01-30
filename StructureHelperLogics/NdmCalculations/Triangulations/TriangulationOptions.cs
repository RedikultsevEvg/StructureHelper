using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class TriangulationOptions : ITriangulationOptions
    {
        public LimitStates LimiteState { get; set; }
        public CalcTerms CalcTerm { get; set; }
    }
}
