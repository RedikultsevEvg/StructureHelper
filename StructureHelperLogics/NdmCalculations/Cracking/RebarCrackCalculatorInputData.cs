using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Class of input data for rebar crack calculator
    /// </summary>
    public class RebarCrackCalculatorInputData : IRebarCrackCalculatorInputData
    {
        /// <inheritdoc/>
        public IRebarCrackInputData? LongRebarData { get; set; }
        /// <inheritdoc/>
        public IRebarCrackInputData? ShortRebarData { get; set; }
        /// <inheritdoc/>
        public IRebarPrimitive RebarPrimitive { get; set; }
        /// <inheritdoc/>
        public IUserCrackInputData? UserCrackInputData { get; set; }
    }
}
