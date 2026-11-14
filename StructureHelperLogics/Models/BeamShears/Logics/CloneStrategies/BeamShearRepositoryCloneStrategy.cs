using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
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

        public IBeamShearRepository GetClone(IBeamShearRepository sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            InitializeStrategies();
            var targetRepository = new BeamShearRepository(id);
            AddChildrenClones(sourceObject, cloneMode, cloneDepth, targetRepository);
            return targetRepository;
        }

        private void AddChildrenClones(IBeamShearRepository sourceObject, CloneMode cloneMode, CloneDepth cloneDepth, IBeamShearRepository targetObject)
        {
            if (cloneDepth == CloneDepth.Deep)
            {
                foreach (var action in sourceObject.Actions)
                {
                    targetObject.Actions.Add(action.GetClone(action, cloneMode, cloneDepth));
                }
                foreach (var section in sourceObject.Sections)
                {
                    targetObject.Sections.Add(section.GetClone(section, cloneMode, cloneDepth));
                }
                foreach (var stirrup in sourceObject.Stirrups)
                {
                    targetObject.Stirrups.Add(stirrup.GetClone(stirrup, cloneMode, cloneDepth));
                }
            }
            else if (cloneDepth == CloneDepth.PreserveReference)
            {
                InitializeStrategies();
                actionUpdateStrategy.Update(targetObject, sourceObject);
                sectionUpdateStrategy.Update(targetObject, sourceObject);
                stirrupUpdateStrategy.Update(targetObject, sourceObject);
                calculatorUpdateStrategy.Update(targetObject, sourceObject);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloneDepth) + ": cloning depth is not right");
            }
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
