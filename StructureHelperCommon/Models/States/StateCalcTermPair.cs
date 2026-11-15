using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.States
{
    /// <inheritdoc/>
    public class StateCalcTermPair : IStateCalcTermPair
    {
        /// <inheritdoc/>
        public LimitStates LimitState { get; set; }
        /// <inheritdoc/>
        public CalcTerms CalcTerm { get; set; }

        public Guid Id { get; set; }

        public StateCalcTermPair(Guid id)
        {
            Id = id;
        }

        public IStateCalcTermPair GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            StateCalcTermPair newItem = new(id);
            new StateCalcTermPairUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
