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
        public double BottomBase { get; set; }
        /// <inheritdoc/>
        public double TopBase { get; set; }
        /// <inheritdoc/>
        public double Height { get; set; }
        /// <inheritdoc/>
        public double TopBaseOffset { get; set; }


        public TrapezoidShape() : this (Guid.NewGuid())
        {
            
        }

        public TrapezoidShape(Guid id)
        {
            Id = id;
        }
    }
}
