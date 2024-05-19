using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class TupleCrackResult : IResult
    {
        public bool IsValid { get; set; }
        public string Description { get; set; }
        public TupleCrackInputData InputData { get; set; }
        public bool IsCracked { get; set; }
        public List<RebarCrackResult>  RebarResults { get; private set; }
        public CrackWidthRebarTupleResult LongTermResult { get; set; }
        public CrackWidthRebarTupleResult ShortTermResult { get; set; }
        //public double MaxLongTermCrackWidth => RebarResults.Select(x => x.LongTermResult.CrackWidth).Max();
        //public double MaxShortTermCrackWidth => RebarResults.Select(x => x.ShortTermResult.CrackWidth).Max();
        //public bool IsLongCrackLessThanUltimate => MaxLongTermCrackWidth <= InputData.UserCrackInputData.UltimateLongCrackWidth;
        //public bool IsShortCrackLessThanUltimate => MaxShortTermCrackWidth <= InputData.UserCrackInputData.UltimateShortCrackWidth;

        public TupleCrackResult()
        {
            RebarResults = new ();
        }
    }
}
