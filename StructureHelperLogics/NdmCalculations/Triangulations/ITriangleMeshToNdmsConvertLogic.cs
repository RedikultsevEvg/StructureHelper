using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using TriangleNet.Meshing;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <summary>
    /// Convert triangle mesh to list of triangle ndm parts
    /// </summary>
    public interface ITriangleMeshToNdmsConvertLogic
    {
        /// <summary>
        /// Options of triangulation
        /// </summary>
        IShapeTriangulationLogicOptions Options { get; set; }
        /// <summary>
        /// Converts mesh to collection of triangle ndm parts with assigned material
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="material"></param>
        /// <returns></returns>
        List<INdm> GetNdmsByMesh(IMesh mesh, IMaterial material);
    }
}
