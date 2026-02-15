using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Infrastructure.Geometry;
using TriangleNet.Meshing;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class TriangleMeshToNdmsConvertLogic : ITriangleMeshToNdmsConvertLogic
    {
        public IShapeTriangulationLogicOptions Options { get; set; }

        public List<INdm> GetNdmsByMesh(IMesh mesh, IMaterial material)
        {
            List<INdm> ndmCollection = [];
            foreach (var triangle in mesh.Triangles)
            {
                TriangleNdm ndm = GetTriangleNdm(material, triangle);
                ndmCollection.Add(ndm);
            }
            TriangulationService.SetPrestrain(ndmCollection, Options.Prestrain);
            return ndmCollection;
        }

        private static TriangleNdm GetTriangleNdm(IMaterial material, TriangleNet.Topology.Triangle triangle)
        {
            List<IPointLd2D> points = [];
            for (int i = 0; i < 3; i++)
            {
                var vertex = triangle.GetVertex(i);
                points.Add(new PointLd2D()
                {
                    X = vertex.X,
                    Y = vertex.Y
                });

            }
            var ndm = new TriangleNdm
            {
                Point1 = points[0],
                Point2 = points[1],
                Point3 = points[2],
                Material = material
            };
            return ndm;
        }

        private static void GetRectangleNdm(TriangleNdm ndm)
        {
            var ndm2 = new RectangleNdm()
            {
                Width = Math.Sqrt(ndm.Area),
                Height = Math.Sqrt(ndm.Area),
                CenterX = ndm.CenterX,
                CenterY = ndm.CenterY,
                Material = ndm.Material
            };
        }
    }
}
