using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.States
{
    /// <summary>
    /// Implements pair of limit state and calculeation term
    /// </summary>
    public interface IStateCalcTermPair
    {
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
    }
}
