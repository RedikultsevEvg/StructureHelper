using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class ConstructionStage : IConstructionStage
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public IConstructionStageInputData InputData { get; set; }

        public ConstructionStage(Guid id)
        {
            Id = id;
            InputData = new ConstructionStageInputData(Guid.NewGuid(), this);
        }

        public IConstructionStage GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ConstructionStage newItem = new(id);
            ConstructionStageUpdateStrategy updateStrategy = new() { UpdateChildren = true };
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
