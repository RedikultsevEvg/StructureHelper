using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc/>
    public class CenterShape : ICenterShape
    {
        /// <inheritdoc/>
        public IPoint2D Center {  get; }
        /// <inheritdoc/>
        public IShape Shape {  get; }
        /// <inheritdoc/>
        public double AngleRadians { get; } = 0.0;

        public CenterShape(IPoint2D center, IShape shape)
        {
            Center = center;
            Shape = shape;
        }

        public CenterShape(IPoint2D center, IShape shape, double rotationAngle) : this(center, shape)
        {
            AngleRadians = rotationAngle;
        }
    }
}
