using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class HasCalculatorsUpdateCloneStrategy : IUpdateStrategy<IHasCalculators>
    {
        private readonly ICloningStrategy cloningStrategy;
        private ICloneStrategy<ICalculator> calculatorCloneStrategy;
        private ICloneStrategy<ICalculator> CalculatorCloneStrategy => calculatorCloneStrategy ??= new CalculatorCloneStrategy(cloningStrategy);

        public HasCalculatorsUpdateCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHasCalculators targetObject, IHasCalculators sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Calculators.Clear();
            foreach (var calculator in sourceObject.Calculators)
            {
                ICalculator newCalculator;
                newCalculator = CalculatorCloneStrategy.GetClone(calculator);
                targetObject.Calculators.Add(newCalculator);
            }
        }
    }
}
