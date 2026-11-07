using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;

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
        private IUpdateStrategy<IHasCalculators> calculatorUpdateStrategy;

        public BeamShearRepositoryCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }
        public IBeamShearRepository GetClone(IBeamShearRepository sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            InitializeStrategies();
            var targetRepository = new BeamShearRepository(id);
            actionUpdateStrategy.Update(targetRepository, sourceObject);
            sectionUpdateStrategy.Update(targetRepository, sourceObject);
            stirrupUpdateStrategy.Update(targetRepository, sourceObject);
            calculatorUpdateStrategy.Update(targetRepository, sourceObject);
            return targetRepository;
        }

        private void InitializeStrategies()
        {
            actionUpdateStrategy ??= new HasActionsUpdateCloneStrategy(cloningStrategy);
            sectionUpdateStrategy ??= new HasSectionsUpdateCloneStrategy(cloningStrategy);
            stirrupUpdateStrategy ??= new HasStirrupsUpdateCloneStrategy(cloningStrategy);
            calculatorUpdateStrategy ??= new HasCalculatorsUpdateCloneStrategy(cloningStrategy);
        }
    }
}
