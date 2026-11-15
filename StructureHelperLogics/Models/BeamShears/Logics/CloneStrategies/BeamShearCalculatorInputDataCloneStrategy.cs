using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Analyses.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class BeamShearCalculatorInputDataCloneStrategy : ICloneStrategy<IBeamShearCalculatorInputData>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IHasBeamShearActions> actionUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> sectionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups> stirrupUpdateStrategy;
        public BeamShearCalculatorInputDataCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public IBeamShearCalculatorInputData GetClone(IBeamShearCalculatorInputData sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            BeamShearCalculatorInputData inputData = new(id);
            AddChildrenClones(sourceObject, cloneMode, cloneDepth, inputData);
            return inputData;
        }

        private void AddChildrenClones(IBeamShearCalculatorInputData sourceObject, CloneMode cloneMode, CloneDepth cloneDepth, BeamShearCalculatorInputData inputData)
        {
            if (cloneDepth == CloneDepth.Deep)
            {
                foreach (var action in sourceObject.Actions)
                {
                    inputData.Actions.Add(action.GetClone(cloneMode, cloneDepth));
                }
                foreach (var section in sourceObject.Sections)
                {
                    inputData.Sections.Add(section.GetClone(cloneMode, cloneDepth));
                }
                foreach (var stirrup in sourceObject.Stirrups)
                {
                    inputData.Stirrups.Add(stirrup.GetClone(cloneMode, cloneDepth));
                }
            }
            else if (cloneDepth == CloneDepth.PreserveReference)
            {
                InitializeStrategies();
                actionUpdateStrategy.Update(inputData, sourceObject);
                sectionUpdateStrategy.Update(inputData, sourceObject);
                stirrupUpdateStrategy.Update(inputData, sourceObject);
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
        }
    }
}
