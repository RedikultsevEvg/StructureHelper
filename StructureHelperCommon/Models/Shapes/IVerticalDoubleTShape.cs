using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IVerticalDoubleTShape : IShape
    {
        /// <summary>
        /// Total height of section (overall depth), m
        /// </summary>
        double FullHeight { get; set; }

        /// <summary>
        /// Width of top flange, m
        /// </summary>
        double TopFlangeWidth { get; set; }

        /// <summary>
        /// Thickness of top flange, m
        /// </summary>
        double TopFlangeThickness { get; set; }

        /// <summary>
        /// Width of bottom flange, m
        /// </summary>
        double BottomFlangeWidth { get; set; }

        /// <summary>
        /// Thickness of bottom flange, m
        /// </summary>
        double BottomFlangeThickness { get; set; }

        /// <summary>
        /// Thickness of web, m
        /// </summary>
        double WebThickness { get; set; }
    }
}
