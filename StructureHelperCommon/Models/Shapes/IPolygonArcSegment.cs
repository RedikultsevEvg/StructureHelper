using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Implements arc segment of polygon
    /// </summary>
    public interface IPolygonArcSegment : IPolygonSegment
    {
        /// <summary>
        /// Vertex of center of arc
        /// </summary>
        IVertex Center { get; }
        /// <summary>
        /// Radius of arc segment, meters
        /// </summary>
        double Radius { get; }
        /// <summary>
        /// Start angle, Radians
        /// </summary>
        double StartAngle { get; }
        /// <summary>
        /// Sweep angle, Radians
        /// </summary>
        double SweepAngle { get; }
    }
}
