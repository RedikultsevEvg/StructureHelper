using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShear : IBeamShear
    {
        private ICloneStrategy<IBeamShear> cloneStrategy;
        public Guid Id { get; }
        public IBeamShearRepository Repository { get; set; } = new BeamShearRepository(Guid.NewGuid());
        public BeamShear(Guid id)
        {
            Id = id;
        }

        public IBeamShear GetClone(IBeamShear sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            var cloningStrategy = new DeepCloningStrategy();
            cloneStrategy = new BeamShearCloneStrategy(cloningStrategy);
            var newItem = cloneStrategy.GetClone(this);
            return newItem;
        }
    }
}
