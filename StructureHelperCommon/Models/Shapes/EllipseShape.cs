using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc/>
    public class EllipseShape : IEllipseShape
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double Width { get; set; }
        /// <inheritdoc/>
        public double Height { get; set; }

        public EllipseShape() : this(Guid.NewGuid())
        {
            
        }
        public EllipseShape(Guid id)
        {
            Id = id;
        }
    }
}
