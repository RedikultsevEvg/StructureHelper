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
    public interface IBeamShearSectionLogicInputData : IInputData
    {
        IBeamShearSection BeamShearSection { get; set; }
        IStirrup Stirrup { get; set; }
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        IForceTuple ForceTuple { get; set; }
    }
}
