using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
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

        public object Clone()
        {
            ConstructionStageUpdateStrategy updateStrategy  = new() { UpdateChildren = true };
            ConstructionStage newItem = new(Guid.NewGuid());
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
