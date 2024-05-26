using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2024 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Result of crack calculation for specific force tuple
    /// </summary>
    public class TupleCrackResult : IResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string Description { get; set; }
        public TupleCrackInputData InputData { get; set; }
        public bool IsCracked { get; set; }
        public List<RebarCrackResult>  RebarResults { get; private set; }
        public CrackWidthRebarTupleResult LongTermResult { get; set; }
        public CrackWidthRebarTupleResult ShortTermResult { get; set; }

        public TupleCrackResult()
        {
            RebarResults = new ();
        }
    }
}
