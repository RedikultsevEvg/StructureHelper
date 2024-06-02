using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangulationLogicOptions
    {
        ITriangulationOptions triangulationOptions { get; set; }
        StrainTuple Prestrain { get; set; }
        IHeadMaterial HeadMaterial { get; set; }
    }
}
