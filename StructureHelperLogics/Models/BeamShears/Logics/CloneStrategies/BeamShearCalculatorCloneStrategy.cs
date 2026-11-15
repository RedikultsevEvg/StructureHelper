using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class BeamShearCalculatorCloneStrategy : ICloneStrategy<IBeamShearCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IBeamShearCalculator> updateStrategy;
        private IUpdateStrategy<IBeamShearCalculator> UpdateStrategy => updateStrategy ??= new BeamShearCalculatorUpdateStrategy();
        private ICloneStrategy<IBeamShearCalculatorInputData> inputDataCloningStrategy;
        private ICloneStrategy<IBeamShearCalculatorInputData> InputDataCloningStrategy => inputDataCloningStrategy ??= new BeamShearCalculatorInputDataCloneStrategy(cloningStrategy);

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
            BeamShearCalculator calculator = new(id);
            UpdateStrategy.Update(calculator, sourceObject);
            calculator.InputData = InputDataCloningStrategy.GetClone(sourceObject.InputData, cloneMode, CloneDepth.PreserveReference);
            return calculator;
        }
    }
}
