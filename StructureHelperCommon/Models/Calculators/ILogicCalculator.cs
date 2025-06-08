using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Calculators
{
    public interface ILogicCalculator : ILogic, ISaveable, ICloneable
    {
        /// <summary>
        /// Method for calculating
        /// </summary>
        void Run();
        /// <summary>
        /// Result of Calculations
        /// </summary>
        IResult Result { get; }
    }
}