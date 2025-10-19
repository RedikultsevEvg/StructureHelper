using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Implements properties of polygon with diferent types of segment 
    /// </summary>
    public interface IPolygonShape : ISaveable
    {
        /// <summary>
        /// Collection of sements of polygon
        /// </summary>
        List<IPolygonSegment> Segments { get; }
    }
}
