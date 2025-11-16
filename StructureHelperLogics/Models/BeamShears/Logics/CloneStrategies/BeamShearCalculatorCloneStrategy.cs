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

        public IBeamShearCalculator GetClone(IBeamShearCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            InitializeStrategies();
            BeamShearCalculator calculator = new(Guid.NewGuid());
            updateStrategy.Update(calculator, sourceObject);
            calculator.InputData = inputDataCloningStrategy.GetClone(sourceObject.InputData);
            return calculator;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearCalculatorUpdateStrategy();
            inputDataCloningStrategy ??= new BeamShearCalculatorInputDataCloneStrategy(cloningStrategy);
        }
    }
}
