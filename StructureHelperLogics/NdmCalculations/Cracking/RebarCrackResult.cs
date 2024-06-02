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
    /// Result of calculation of crack for specific result
    /// </summary>
    public class RebarCrackResult : IResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string Description { get; set; }
        /// <summary>
        /// Specific rebar primitive
        /// </summary>
        public RebarPrimitive RebarPrimitive { get; set; }
        /// <summary>
        /// Result of calculation of crack for long term
        /// </summary>
        public CrackWidthRebarTupleResult LongTermResult { get; set; }
        /// <summary>
        /// Result of calculation of crack for short term
        /// </summary>
        public CrackWidthRebarTupleResult ShortTermResult { get; set; }
    }
}
