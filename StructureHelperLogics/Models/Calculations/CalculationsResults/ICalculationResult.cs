using LoaderCalculator.Data.ResultData;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Calculations.CalculationsResults
{
    /// <summary>
    /// Represent result of calculation of ndm analisys
    /// </summary>
    public interface ICalculationResult
    {
        /// <summary>
        /// True if result of calculation is valid
        /// </summary>
        bool IsValid { get; }
        /// <summary>
        /// Text of result of calculations
        /// </summary>
        string Desctription { get; }
        /// <summary>
        /// Keep result of calculations from ndm-library
        /// </summary>
        ILoaderResults LoaderResults { get; }
    }
}
