using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangulationLogicOptions
    {
        StrainTuple Prestrain { get; set; }
    }
}
