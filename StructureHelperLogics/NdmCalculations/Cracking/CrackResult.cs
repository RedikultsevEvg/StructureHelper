using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackResult : IResult
    {
        public bool IsValid { get; set; }
        public string? Description { get; set; }
        public List<TupleCrackResult> TupleResults {get;set;}
        public CrackResult()
        {
            TupleResults = new();
        }
    }
}
