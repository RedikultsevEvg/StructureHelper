using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements properties for calculting maximum range for considering of inclined sections
    /// </summary>
    public interface IBeamShearDesignRangeProperty : ISaveable, ICloneable
    {
        /// <summary>
        /// Absolute value of range in metres
        /// </summary>
        double AbsoluteRangeValue { get; set; }
        /// <summary>
        /// Relative value of range as factor of effective depth of cross-section
        /// </summary>
        double RelativeEffectiveDepthRangeValue { get; set; }
        /// <summary>
        /// Number of step of solvation
        /// </summary>
        int StepCount { get; set; }
    }
}
