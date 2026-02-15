using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Infrastructure.Geometry;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using TriangleNet.Geometry;
using TriangleNet.Meshing;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <summary>
    /// Logic for triangulation of line poligon shapr into collection of ndm parts
    /// </summary>
    public class ShapeTriangulationLogic : ITriangulationLogic
    {
        private ShapeTriangulationLogicOption options;

        public ShapeTriangulationLogic(ShapeTriangulationLogicOption triangulationLogicOption)
        {
            this.options = triangulationLogicOption;
        }

        /// <inheritdoc/>
        public IEnumerable<INdm> GetNdmCollection()
        {
            IMesh mesh = GetMesh();
            var material = options.HeadMaterial.GetLoaderMaterial(options.TriangulationOptions.LimiteState, options.TriangulationOptions.CalcTerm);
            List<INdm> ndms = GetNdmsByMesh(mesh, material);
            return ndms;
        }

        private List<INdm> GetNdmsByMesh(IMesh mesh, IMaterial material)
        {
            var logic = new TriangleMeshToNdmsConvertLogic()
            {
                Options = options
            };
            var ndms = logic.GetNdmsByMesh(mesh, material);
            return ndms;
        }

        private IMesh GetMesh()
        {
            var logic = new MeshShapeLogic()
            {
                CenterShape = new CenterShape(options.Center, options.Shape),
                MaximumMeshSize = options.DivisionSize.NdmMaxSize,
                MinimumAngleInDegree = 25.0
            };
            return logic.Triangulate();
        }

        /// <inheritdoc/>
        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
