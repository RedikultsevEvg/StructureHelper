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
    public class RebarCrackCalculatorInputData : IInputData
    {
        /// <summary>
        /// Long term rebar data
        /// </summary>
        public RebarCrackInputData? LongRebarData { get; set; }
        /// <summary>
        /// Short term rebar data
        /// </summary>
        public RebarCrackInputData? ShortRebarData { get; set; }
        public RebarPrimitive RebarPrimitive { get; set; }
        public UserCrackInputData UserCrackInputData { get; set; }
    }
}
