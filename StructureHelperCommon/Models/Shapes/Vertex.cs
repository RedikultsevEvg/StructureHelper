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
        /// <summary>
        /// Creates new vertex with id = new Guid
        /// </summary>
        /// <param name="x">Coordinate x of vertex</param>
        /// <param name="y">Coordinate y of vertex</param>
        public Vertex(double x, double y) : this(Guid.NewGuid())
        {
            Point = new Point2D() { X = x, Y = y };
        }

        public IPoint2D Point { get; set; } = new Point2D(Guid.NewGuid()) { X = 0, Y = 0};

    }

}
