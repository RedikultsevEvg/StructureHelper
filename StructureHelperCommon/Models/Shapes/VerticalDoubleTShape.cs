using System;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc/>
    public class VerticalDoubleTShape : IVerticalDoubleTShape
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double FullHeight { get; set; }
        /// <inheritdoc/>
        public double TopFlangeWidth { get; set; }
        /// <inheritdoc/>
        public double TopFlangeThickness { get; set; }
        /// <inheritdoc/>
        public double BottomFlangeWidth { get; set; }
        /// <inheritdoc/>
        public double BottomFlangeThickness { get; set; }
        /// <inheritdoc/>
        public double WebThickness { get; set; }


        public VerticalDoubleTShape() : this (Guid.NewGuid())
        {
            
        }
        public VerticalDoubleTShape(Guid id)
        {
            Id = id;
        }
    }
}
