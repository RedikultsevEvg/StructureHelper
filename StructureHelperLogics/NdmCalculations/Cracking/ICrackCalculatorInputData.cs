using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Input data for crack calculator
    /// </summary>
    public interface ICrackCalculatorInputData : IInputData, IHasPrimitives, IHasForceActions, ISaveable
    {
        /// <summary>
        /// Used difined data for crack width calculation
        /// </summary>
        IUserCrackInputData UserCrackInputData { get; set; }
    }
}