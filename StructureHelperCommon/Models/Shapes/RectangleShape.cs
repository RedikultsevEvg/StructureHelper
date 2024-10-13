using System;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc />
    public class RectangleShape : IRectangleShape
    {
        public Guid Id { get; }
        /// <inheritdoc />
        public double Width { get; set; }
        /// <inheritdoc />
        public double Height { get; set; }
        /// <inheritdoc />
        public double Angle { get; set; }

        public RectangleShape(Guid id)
        {
            Id = id;
        }
        public RectangleShape() : this (Guid.NewGuid())
        {
            
        }
    }
}
