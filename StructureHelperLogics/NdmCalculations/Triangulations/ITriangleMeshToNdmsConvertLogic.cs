using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using TriangleNet.Meshing;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangleMeshToNdmsConvertLogic
    {
        IShapeTriangulationLogicOptions Options { get; set; }
        List<INdm> GetNdmsByMesh(IMesh mesh, IMaterial material);
    }
}
