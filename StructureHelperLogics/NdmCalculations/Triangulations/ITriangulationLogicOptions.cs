using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangulationLogicOptions
    {
        ITriangulationOptions TriangulationOptions { get; set; }
        StrainTuple InitialPrestrain { get; set; }
        StrainTuple HistoryPrestrain { get; set; }
        IHeadMaterial HeadMaterial { get; set; }
    }
}
