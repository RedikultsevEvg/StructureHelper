using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implement properties for concentrated load in beam
    /// </summary>
    public interface IConcentratedForce : IBeamSpanLoad
    {
        /// <summary>
        /// Coordinate of location of force along beam, m
        /// </summary>
        double ForceCoordinate { get; set; }
        /// <summary>
        /// Value of force, N
        /// </summary>
        double ForceValue { get; set; }
    }
}
