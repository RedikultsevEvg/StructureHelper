using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class LinePolygonShapeUpdateStrategy : IParentUpdateStrategy<ILinePolygonShape>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(ILinePolygonShape targetObject, ILinePolygonShape sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }

            // Update simple properties
            targetObject.IsClosed = sourceObject.IsClosed;
            if (UpdateChildren == true)
            {
                // Replace vertices
                targetObject.Clear();
                foreach (var vertex in sourceObject.Vertices)
                {
                    // Copy the underlying point into a new vertex
                    Vertex newVertex = GetVertexClone(vertex);
                    targetObject.AddVertex(newVertex);
                }
            }
        }

        private static Vertex GetVertexClone(IVertex vertex)
        {
            var updateStrategy = new VertexUpdateStrategy();
            Vertex newVertex = new(Guid.NewGuid());
            updateStrategy.Update(newVertex, vertex);
            return newVertex;
        }
    }

}
