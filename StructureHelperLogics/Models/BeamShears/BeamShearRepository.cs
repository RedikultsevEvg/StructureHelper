using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearRepository : IBeamShearRepository
    {
        private ICloneStrategy<IBeamShearRepository> cloneStrategy;
        public Guid Id { get; }
        public List<IBeamShearAction> Actions { get; } = new();
        public List<IBeamShearSection> Sections { get; } = new();
        public List<IStirrup> Stirrups { get; } = new();
        public List<ICalculator> Calculators { get; } = new();


        public BeamShearRepository(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            var cloningStrategy = new DeepCloningStrategy();
            cloneStrategy = new BeamShearRepositoryCloneStrategy(cloningStrategy);
            var newItem = cloneStrategy.GetClone(this);
            return newItem;
        }
    }
}
