using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements input data for calculating bearing capacity of cross-section of RC member for shear
    /// </summary>
    public interface IBeamShearSectionLogicInputData : IInputData, ISaveable
    {
        IBeamShearAction BeamShearAction { get; set; }
        IBeamShearSection BeamShearSection { get; set; }
        /// <summary>
        /// Properties of RC cross-section
        /// </summary>
        IInclinedSection InclinedSection { get; set; }
        IInclinedSection InclinedCrack { get; set; }
        /// <summary>
        /// Properties of stirrups in cross-section
        /// </summary>
        IStirrup Stirrup { get; set; }
        /// <summary>
        /// Limit state for calculating
        /// </summary>
        LimitStates LimitState { get; set; }
        /// <summary>
        /// Term (Duration) for calculation
        /// </summary>
        CalcTerms CalcTerm { get; set; }
        /// <summary>
        /// Force tuple for calculation
        /// </summary>
        IForceTuple ForceTuple { get; set; }
    }
}
