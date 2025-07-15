using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements properies of stirrup by inclined rebar
    /// </summary>
    public interface IStirrupByInclinedRebar : IStirrup
    {
        /// <summary>
        /// Coordinate of start point of inclination, m
        /// </summary>
        double StartCoordinate { get; set; }
        /// <summary>
        /// Distance beetwen start/end point and point where rebar work is started 
        /// </summary>
        double OffSet { get; set; }
        /// <summary>
        /// Angle of inclination of rebar in degrees
        /// </summary>
        double AngleOfInclination { get; set; }
        /// <summary>
        /// Number of legs of inclinated rebar
        /// </summary>
        double LegCount { get; set; }
        /// <summary>
        /// Cross-section of rebar
        /// </summary>
        IRebarSection RebarSection { get; set; }
    }
}
