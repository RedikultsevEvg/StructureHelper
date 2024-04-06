using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthCalculatorResult : IResult
    {
        public bool IsValid { get; set; }
        public string Description { get; set; }
        public IForceTuple LongTermTuple { get; set; }
        public IForceTuple ShortTermTuple { get; set; }
        public bool IsCracked { get; set; }
        public List<CrackWidthSimpleCalculatorResult>  RebarResults { get; set; }
        public double MaxLongTermCrackWidth => IsCracked? RebarResults.Select(x => x.LongTermResult.CrackWidth).Max() : 0d;
        public double MaxShortTermCrackWidth => IsCracked? RebarResults.Select(x => x.ShortTermResult.CrackWidth).Max() : 0d;

        public CrackWidthCalculatorResult()
        {
            RebarResults = new List<CrackWidthSimpleCalculatorResult>();
        }
    }
}
