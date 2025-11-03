using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.States
{
    /// <inheritdoc/>
    public class StateCalcTermPair : IStateCalcTermPair
    {
        /// <inheritdoc/>
        public LimitStates LimitState { get; set; }
        /// <inheritdoc/>
        public CalcTerms CalcTerm { get; set; }
    }
}
