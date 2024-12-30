using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class TupleCrackResult : ITupleCrackResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string? Description { get; set; }
        /// <inheritdoc/>
        public TupleCrackInputData? InputData { get; set; }
        /// <inheritdoc/>
        public bool IsCracked { get; set; }
        /// <inheritdoc/>
        public List<IRebarCrackResult>? RebarResults { get; private set; } = new();
        /// <inheritdoc/>
        public CrackWidthRebarTupleResult? LongTermResult { get; set; }
        /// <inheritdoc/>
        public CrackWidthRebarTupleResult? ShortTermResult { get; set; }
    }
}
