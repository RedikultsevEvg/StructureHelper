using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Calculators
{
    public interface ICalculator : ILogic, ISaveable, ICloneable
    {      
        string Name { get; set; }
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
