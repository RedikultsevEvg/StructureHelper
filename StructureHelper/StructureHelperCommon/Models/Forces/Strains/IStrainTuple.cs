using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Interface for generic curvature for beams
    /// </summary>
    public interface IStrainTuple : ICloneable
    {
        /// <summary>
        /// Curvature about x-axis
        /// </summary>
        double Kx { get; set; }
        /// <summary>
        /// Curvature about y-axis
        /// </summary>
        double Ky { get; set; }
        /// <summary>
        /// Strain along z-axis
        /// </summary>
        double EpsZ { get; set; }
        /// <summary>
        /// Screw along x-axis
        /// </summary>
        double Gx { get; set; }
        /// <summary>
        /// Screw along y-axis
        /// </summary>
        double Gy { get; set; }
        /// <summary>
        /// Twisting about z-axis
        /// </summary>
        double Gz { get; set; }
    }
}
