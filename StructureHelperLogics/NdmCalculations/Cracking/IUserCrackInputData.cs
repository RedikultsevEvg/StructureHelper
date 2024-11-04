using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// User settings for crack width calculation
    /// </summary>
    public interface IUserCrackInputData : IInputData, ISaveable
    {
        /// <summary>
        /// Flag of assigning of length betweenCracks by user or auto 
        /// </summary>
        bool SetLengthBetweenCracks { get; set; }
        /// <summary>
        /// User value of length between cracks
        /// </summary>
        double LengthBetweenCracks { get; set; }
        /// <summary>
        /// Flag of assigning of softening factor by user or auto 
        /// </summary>
        bool SetSofteningFactor { get; set; }
        /// <summary>
        /// User value of softening factor
        /// </summary>
        double SofteningFactor { get; set; }
        /// <summary>
        /// Ultimate value of crack width due to long term loads, m
        /// </summary>
        double UltimateLongCrackWidth { get; set; }
        /// <summary>
        /// Ultimate value of crack width due to short term loads, m
        /// </summary>
        double UltimateShortCrackWidth { get; set; }
    }
}