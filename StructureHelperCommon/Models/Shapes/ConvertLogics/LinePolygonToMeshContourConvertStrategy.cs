using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;

namespace StructureHelperCommon.Models.Shapes
{
    public class LinePolygonToMeshContourConvertStrategy : IObjectConvertStrategy<Contour, ILinePolygonShape>
    {
        public Contour Convert(ILinePolygonShape source)
        {
            List<IVertex> vertices = source.Vertices.ToList();
            var triangleVertices = new List<TriangleNet.Geometry.Vertex>();
            foreach (var vertex in vertices)
            {
                triangleVertices.Add(new TriangleNet.Geometry.Vertex(vertex.Point.X, vertex.Point.Y));

            }
            // Add contour to polygon — this automatically defines the connecting segments
            Contour contour = new(triangleVertices);
            return contour;
        }
    }
}
