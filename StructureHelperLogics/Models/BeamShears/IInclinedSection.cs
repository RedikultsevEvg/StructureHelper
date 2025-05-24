using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement parameers of inclined cross-section for beam shear calculating
    /// </summary>
    public interface IInclinedSection : IEffectiveDepth
    {
        /// <summary>
        /// Width of cross-section
        /// </summary>
        double WebWidth { get; set; }
        /// <summary>
        /// Coordinate of start of inclined cross-section
        /// </summary>
        double StartCoord { get; set; }
        /// <summary>
        /// Coordinate of end of inclined cross-section
        /// </summary>
        double EndCoord { get; set; }
        /// <summary>
        /// Strength of concrete in compression, Pa
        /// </summary>
        double ConcreteCompressionStrength { get; set; }
        /// <summary>
        /// Strength of concrete in tension, Pa
        /// </summary>
        double ConcreteTensionStrength { get; set; }

    }
}
