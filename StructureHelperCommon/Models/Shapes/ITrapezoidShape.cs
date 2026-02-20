using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public interface ITrapezoidShape : IShape
    {
        /// <summary>
        /// Bottom base length (parallel to top base), m
        /// </summary>
        double BottomBase { get; set; }

        /// <summary>
        /// Top base length (parallel to bottom base), m
        /// </summary>
        double TopBase { get; set; }

        /// <summary>
        /// Height between bases, m
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// Horizontal offset of top base relative to bottom base center, m.
        /// Positive value shifts top base to the right.
        /// </summary>
        double TopBaseOffset { get; set; }
    }
}
