using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    /// <inheritdoc/>
    public class TrapezoidShape : ITrapezoidShape
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public double BottomBase { get; set; } = 0.3;
        /// <inheritdoc/>
        public double TopBase { get; set; } = 0.4;
        /// <inheritdoc/>
        public double Height { get; set; } = 0.6;
        /// <inheritdoc/>
        public double TopBaseOffset { get; set; } = 0.0;


        public TrapezoidShape() : this (Guid.NewGuid())
        {
            
        }

        public TrapezoidShape(Guid id)
        {
            Id = id;
        }
    }
}
