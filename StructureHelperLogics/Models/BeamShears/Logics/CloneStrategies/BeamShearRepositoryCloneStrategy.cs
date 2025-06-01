using StructureHelperCommon.Infrastructures.Interfaces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearRepositoryCloneStrategy : ICloneStrategy<IBeamShearRepository>
    {
        private ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IHasBeamShearActions> actionUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> sectionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups> stirrupUpdateStrategy;
        private BeamShearRepository targetRepository;

        public BeamShearRepositoryCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }
        public IBeamShearRepository GetClone(IBeamShearRepository sourceObject)
        {
            InitializeStrategies();
            targetRepository = new(Guid.NewGuid());
            actionUpdateStrategy.Update(targetRepository, sourceObject);
            sectionUpdateStrategy.Update(targetRepository, sourceObject);
            stirrupUpdateStrategy.Update(targetRepository, sourceObject);
            return targetRepository;
        }

        private void InitializeStrategies()
        {
            actionUpdateStrategy ??= new HasActionsUpdateCloneStrategy(cloningStrategy);
            sectionUpdateStrategy ??= new HasSectionsUpdateCloneStrategy(cloningStrategy);
            stirrupUpdateStrategy ??= new HasStirrupsUpdateCloneStrategy(cloningStrategy);
        }
    }
}
