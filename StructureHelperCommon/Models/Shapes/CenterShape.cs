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
        public CenterShape(IPoint2D center, IShape shape)
        {
            Center = center;
            Shape = shape;
        }
    }
}
