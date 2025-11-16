using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.States;

namespace DataAccess.DTOs
{
    public class StateCalcTermPairDTO : IStateCalcTermPair
    {
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
    }
}
