using StructureHelperCommon.Infrastructures.Exceptions;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    public class Vertex : IVertex
    {
        public Guid Id { get; }

        public Vertex(Guid id)
        {
            Id = id;
        }

        public IPoint2D Point { get; set; }

    }

}
