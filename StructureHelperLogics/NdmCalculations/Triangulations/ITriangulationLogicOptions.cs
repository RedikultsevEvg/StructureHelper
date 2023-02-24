using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangulationLogicOptions
    {
        IStrainTuple Prestrain { get; set; }
    }
}
