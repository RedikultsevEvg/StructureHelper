using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IEllipseShape : IShape
    {
        /// <summary>
        /// Width of rectangle, m
        /// </summary>
        double Width { get; set; }
        /// <summary>
        /// Height of rectangle, m
        /// </summary>
        double Height { get; set; }
    }
}
