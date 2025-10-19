using System.Collections.Generic;

namespace StructureHelperCommon.Models.Shapes
{
    public interface ILinePolygonShape : IShape
    {
        IReadOnlyList<IVertex> Vertices { get; }
        bool IsClosed { get; set; }

        IVertex AddVertex(IVertex vertex);
        IVertex InsertVertex(int index, IVertex vertex);
        IVertex AddVertexBefore(IVertex existing, IVertex vertex);
        IVertex AddVertexAfter(IVertex existing, IVertex vertex);

        void RemoveVertex(IVertex vertex);
        void Clear();
    }

}
