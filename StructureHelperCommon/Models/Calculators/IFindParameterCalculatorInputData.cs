using System;

namespace StructureHelperCommon.Models.Calculators
{
    /// <summary>
    /// Input data for calculators of finding parameters
    /// </summary>
    public interface IFindParameterCalculatorInputData : IInputData
    {
        /// <summary>
        /// Start value of range where parameter looking for
        /// </summary>
        double StartValue { get; set; }
        /// <summary>
        /// End value of range where parameter looking for
        /// </summary>
        double EndValue { get; set; }
        /// <summary>
        /// Predicate for checking parameter for true;
        /// </summary>
        Predicate<double> Predicate { get; set; }
    }
}