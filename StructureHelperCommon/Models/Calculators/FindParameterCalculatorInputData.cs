using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    /// <inheritdoc/>
    public class FindParameterCalculatorInputData : IFindParameterCalculatorInputData
    {
        /// <inheritdoc/>
        public double StartValue { get; set; }
        /// <inheritdoc/>
        public double EndValue { get; set; }
        /// <inheritdoc/>
        public Predicate<double> Predicate { get; set; }
        public FindParameterCalculatorInputData()
        {
            StartValue = 0d;
            EndValue = 1d;
        }
    }
}
