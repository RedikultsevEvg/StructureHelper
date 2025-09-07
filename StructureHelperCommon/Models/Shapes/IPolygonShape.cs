using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IPolygonShape : IShape
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
