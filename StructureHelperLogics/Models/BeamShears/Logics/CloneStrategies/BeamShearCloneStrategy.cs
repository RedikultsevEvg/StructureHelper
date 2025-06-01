using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCloneStrategy : ICloneStrategy<IBeamShear>
    {
        private ICloningStrategy cloningStrategy;
        private ICloneStrategy<IBeamShearRepository> cloneStrategy;
        private BeamShear beamShear;
        public BeamShearCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public IBeamShear GetClone(IBeamShear sourceObject)
        {
            InitializeStrategies();
            beamShear = new(Guid.NewGuid())
            {
                Repository = cloneStrategy.GetClone(sourceObject.Repository)
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
