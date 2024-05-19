using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCrackResult : IResult
    {
        public bool IsValid { get; set; }
        public string Description { get; set; }
        public RebarPrimitive RebarPrimitive { get; set; }
        public CrackWidthRebarTupleResult LongTermResult { get; set; }
        public CrackWidthRebarTupleResult ShortTermResult { get; set; }
    }
}
