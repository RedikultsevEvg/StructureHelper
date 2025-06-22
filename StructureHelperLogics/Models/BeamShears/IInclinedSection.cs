using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement parameters of inclined cross-section for beam shear calculating
    /// </summary>
    public interface IInclinedSection : IEffectiveDepth
    {
        /// <summary>
        /// Reference to source beam shear section
        /// </summary>
        IBeamShearSection BeamShearSection { get; set; }
        /// <summary>
        /// Type of limite state for calculating
        /// </summary>
        LimitStates LimitState { get; set; }
        /// <summary>
        /// Type od calc term for calculating
        /// </summary>
        CalcTerms CalcTerm { get; set; }
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
