using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
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

        public IBeamShearCalculatorInputData GetClone(IBeamShearCalculatorInputData sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            InitializeStrategies();
            BeamShearCalculatorInputData inputData = new(Guid.NewGuid());
            actionUpdateStrategy.Update(inputData, sourceObject);
            sectionUpdateStrategy.Update(inputData, sourceObject);
            stirrupUpdateStrategy.Update(inputData, sourceObject);
            return inputData;
        }

        private void InitializeStrategies()
        {
            actionUpdateStrategy ??= new HasActionsUpdateCloneStrategy(cloningStrategy);
            sectionUpdateStrategy ??= new HasSectionsUpdateCloneStrategy(cloningStrategy);
            stirrupUpdateStrategy ??= new HasStirrupsUpdateCloneStrategy(cloningStrategy);
        }
    }
}
