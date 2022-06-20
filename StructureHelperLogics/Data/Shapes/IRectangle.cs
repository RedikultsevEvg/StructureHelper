using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.Shapes
{
    public interface IRectangle
    {
        /// <summary>
        /// Width of rectangle, m
        /// </summary>
        double Width { get; }
        /// <summary>
        /// Height of rectangle, m
        /// </summary>
        double Height { get; }
        /// <summary>
        /// Angle of rotating rectangle, rad
        /// </summary>
        double Angle { get; }
    }
}
