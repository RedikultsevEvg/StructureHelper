using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class RebarCrackResult : IRebarCrackResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string? Description { get; set; }
        /// <inheritdoc/>
        public IRebarNdmPrimitive? RebarPrimitive { get; set; }
        /// <inheritdoc/>
        public CrackWidthRebarTupleResult? LongTermResult { get; set; }
        /// <inheritdoc/>
        public CrackWidthRebarTupleResult? ShortTermResult { get; set; }
    }
}
