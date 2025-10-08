using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Input data fo roce tuple calculator
    /// </summary>
    public interface IForceCalculatorInputData : IInputData, ISaveable, IHasPrimitives, IHasForceActions
    {
        /// <summary>
        /// Accuracy of calculating
        /// </summary>
        IAccuracy Accuracy { get; set; }
        /// <summary>
        /// List of limit states, available for calculating
        /// </summary>
        List<LimitStates> LimitStatesList { get; }
        /// <summary>
        /// List of type of duration, available for calculation
        /// </summary>
        List<CalcTerms> CalcTermsList { get; }
        /// <summary>
        /// Settings for calculating of stability fo compressed members
        /// </summary>
        ICompressedMember CompressedMember { get; set; }
        bool CheckStrainLimit { get; set; }
    }
}