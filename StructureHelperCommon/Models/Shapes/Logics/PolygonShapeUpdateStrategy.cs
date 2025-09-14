using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace StructureHelperCommon.Models.Shapes
{
    public class PolygonShapeUpdateStrategy : IUpdateStrategy<IPolygonShape>
    {
        public void Update(IPolygonShape targetObject, IPolygonShape sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }

            // Update simple properties
            targetObject.IsClosed = sourceObject.IsClosed;

            // Replace vertices
            targetObject.Clear();
            foreach (var vertex in sourceObject.Vertices)
            {
                // Copy the underlying point into a new vertex
                Vertex newVertex = GetVertexClone(vertex);
                targetObject.AddVertex(newVertex);
            }
        }

        private static Vertex GetVertexClone(IVertex vertex)
        {
            Point2D newVertexPoint = new(Guid.NewGuid())
            {
                X = vertex.Point.X,
                Y = vertex.Point.Y
            };
            Vertex newVertex = new(Guid.NewGuid())
            {
                Point = newVertexPoint
            };
            return newVertex;
        }
    }

}
