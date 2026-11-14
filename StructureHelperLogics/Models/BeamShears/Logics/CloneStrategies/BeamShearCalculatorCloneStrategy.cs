using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class BeamShearCalculatorCloneStrategy : ICloneStrategy<IBeamShearCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IBeamShearCalculator> updateStrategy;
        private ICloneStrategy<IBeamShearCalculatorInputData> inputDataCloningStrategy;
        public BeamShearCalculatorCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public IBeamShearCalculator GetClone(IBeamShearCalculator sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            InitializeStrategies();
            BeamShearCalculator calculator = new(id);
            updateStrategy.Update(calculator, sourceObject);
            calculator.InputData = inputDataCloningStrategy.GetClone(sourceObject.InputData, cloneMode, CloneDepth.Deep);
            return calculator;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearCalculatorUpdateStrategy();
            inputDataCloningStrategy ??= new BeamShearCalculatorInputDataCloneStrategy(cloningStrategy);
        }
    }
}
