using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Infrastructure.Geometry;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using static System.Windows.Forms.Design.AxImporter;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    /// <summary>
    /// Logic for triangulation of line poligon shapr into collection of ndm parts
    /// </summary>
    public class LinePolygonTriangulationLogic : ITriangulationLogic
    {
        private LinePolygonTriangulationLogicOption options;

        public LinePolygonTriangulationLogic(LinePolygonTriangulationLogicOption triangulationLogicOption)
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
            List<INdm> ndmCollection = [];
            foreach (var triangle in mesh.Triangles)
            {
                List<IPointLd2D> points = [];
                for (int i = 0; i < 3; i++)
                {
                    var vertex1 = triangle.GetVertex(i);
                    points.Add(new PointLd2D() { X = vertex1.X, Y = vertex1.Y });

                }
                var ndm = new TriangleNdm() { Point1 = points[0], Point2 = points[1], Point3 = points[2] };
                ndm.Material = material;
                var ndm2 = new RectangleNdm() { Width = Math.Sqrt(ndm.Area), Height = Math.Sqrt(ndm.Area), CenterX = ndm.CenterX, CenterY = ndm.CenterY, Material = ndm.Material};
                ndmCollection.Add(ndm);
            }
            TriangulationService.SetPrestrain(ndmCollection, options.Prestrain);
            return ndmCollection;
        }

        private IMesh GetMesh()
        {
            if (options.Shape is not ILinePolygonShape polygonShape)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(options.Shape) + ": Shape is not line polygon shape");
            }
            var polygon = new Polygon();
            List<IVertex> vertices = polygonShape.Vertices.ToList();
            var contour = new List<TriangleNet.Geometry.Vertex>();
            foreach (var vertex in vertices)
            {
                contour.Add(new TriangleNet.Geometry.Vertex(vertex.Point.X, vertex.Point.Y));
                
            }
            // Add contour to polygon — this automatically defines the connecting segments
            polygon.Add(new Contour(contour));
            var quality = new QualityOptions()
            {
                MinimumAngle = 25.0,
                MaximumArea = options.DivisionSize.NdmMaxSize * options.DivisionSize.NdmMaxSize,
            };
            var mesh = polygon.Triangulate(quality);
            return mesh;
        }

        /// <inheritdoc/>
        public void ValidateOptions(ITriangulationLogicOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
