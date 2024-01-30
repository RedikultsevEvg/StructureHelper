using LoaderCalculator.Data.ResultData;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Calculations.CalculationsResults
{
    /// <inheritdoc/>
    class CalculationResult : ICalculationResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string Desctription { get; set; }
        /// <inheritdoc/>
        public ILoaderResults LoaderResults { get; set; }
    }
}
