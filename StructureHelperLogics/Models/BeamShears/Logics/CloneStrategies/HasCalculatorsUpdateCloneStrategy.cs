using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class HasCalculatorsUpdateCloneStrategy : IUpdateStrategy<IHasCalculators>
    {
        private readonly ICloningStrategy cloningStrategy;
        private ICloneStrategy<IBeamShearCalculator> beamShearCalculatorCloneStrategy;

        public HasCalculatorsUpdateCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHasCalculators targetObject, IHasCalculators sourceObject)
        {
            CheckObject.IsNull(cloningStrategy);
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Calculators.Clear();
            foreach (var calculator in sourceObject.Calculators)
            {
                ICalculator newCalculator;
                if (calculator is IBeamShearCalculator shearCalculator)
                {
                    beamShearCalculatorCloneStrategy ??= new BeamShearCalculatorCloneStrategy(cloningStrategy);
                    newCalculator = beamShearCalculatorCloneStrategy.GetClone(shearCalculator);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(calculator));
                }
                targetObject.Calculators.Add(newCalculator);
            }
        }
    }
}
