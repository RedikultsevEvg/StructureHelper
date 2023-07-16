using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes.Logics;
using System;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc />
    public class Point2D : IPoint2D
    {
        private readonly IUpdateStrategy<IPoint2D> updateStrategy = new PointShapeUpdateStrategy();
        /// <inheritdoc />
        public Guid Id { get; }
        /// <inheritdoc />
        public double X { get; set; }
        /// <inheritdoc />
        public double Y { get; set; }

        public Point2D(Guid id)
        {
            Id = id;
        }

        public Point2D() : this(Guid.NewGuid()) { }

        public object Clone()
        {
            var newItem = new Point2D();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
