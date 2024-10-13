using System;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc />
    public class LineShape : ILineShape
    {
        /// <inheritdoc />
        public Guid Id { get; }
        /// <inheritdoc />
        public IPoint2D StartPoint { get; set; } = new Point2D();
        /// <inheritdoc />
        public IPoint2D EndPoint { get; set; } = new Point2D();
        /// <inheritdoc />
        public double Thickness { get; set; } = 0d;

        public LineShape(Guid id)
        {
            Id = id;
        }

        public LineShape() : this (Guid.NewGuid())
        {
        }
    }
}
