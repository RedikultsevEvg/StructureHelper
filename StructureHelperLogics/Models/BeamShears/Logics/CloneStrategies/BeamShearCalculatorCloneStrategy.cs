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

        public IBeamShearCalculator GetClone(IBeamShearCalculator sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            InitializeStrategies();
            BeamShearCalculator calculator = new(id);
            updateStrategy.Update(calculator, sourceObject);
            calculator.InputData = inputDataCloningStrategy.GetClone(sourceObject.InputData, cloningMode);
            return calculator;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearCalculatorUpdateStrategy();
            inputDataCloningStrategy ??= new BeamShearCalculatorInputDataCloneStrategy(cloningStrategy);
        }
    }
}
