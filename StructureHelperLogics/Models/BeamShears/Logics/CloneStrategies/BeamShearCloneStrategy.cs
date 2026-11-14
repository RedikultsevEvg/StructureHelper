using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCloneStrategy : ICloneStrategy<IBeamShear>
    {
        private ICloningStrategy cloningStrategy;
        private ICloneStrategy<IBeamShearRepository> cloneStrategy;
        public BeamShearCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public IBeamShear GetClone(IBeamShear sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            InitializeStrategies();
            var beamShear = new BeamShear(id)
            {
                Repository = cloneStrategy.GetClone(sourceObject.Repository, cloneMode, cloneDepth)
            };
            return beamShear;
        }

        private void InitializeStrategies()
        {
            cloningStrategy = new DeepCloningStrategy();
            cloneStrategy ??= new BeamShearRepositoryCloneStrategy(cloningStrategy);
        }
    }
}
