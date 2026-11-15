using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.States;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class StateCalcTermPairDTO : IStateCalcTermPair
    {
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }

        public Guid Id { get; }

        public StateCalcTermPairDTO(Guid id)
        {
            Id = id;
        }

        public IStateCalcTermPair GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            StateCalcTermPairDTO newItem = new(id);
            new StateCalcTermPairUpdateStrategy().Update(newItem, this);
            return newItem;
        }
    }
}
