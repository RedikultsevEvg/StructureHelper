using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class ConstructionStage : IConstructionStage
    {
        private IUpdateStrategy<IConstructionStage> updateStrategy;
        private IUpdateStrategy<IConstructionStage> UpdateStrategy => updateStrategy ??= new ConstructionStageUpdateStrategy() { UpdateChildren = true };
        private const int minSubSepNumber = 3;
        private const double minDuration = 0d;
        private int subStepNumber = 10;
        private double duration;

        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public int SubStepNumber
        {
            get => subStepNumber;
            set
            {
                subStepNumber = Math.Max(value, minSubSepNumber);
            }
        }
        public List<INdmPrimitive> Primitives { get; } = [];
        public IForceAction ForceAction { get; set; }
        public double Duration
        {
            get => duration;
            set
            {
                duration = Math.Max(value, minDuration);
            }
        }

        public ConstructionStage(Guid id)
        {
            Id = id;
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
