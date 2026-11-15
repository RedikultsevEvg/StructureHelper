using LoaderCalculator;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.BeamShears
{
    public class CalculatorCloneStrategy : ICloneStrategy<ICalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private ICloneStrategy<IBeamShearCalculator> beamShearCloneStrategy;
        private ICloneStrategy<IBeamShearCalculator> BeamShearCloneStrategy => beamShearCloneStrategy ??= new BeamShearCalculatorCloneStrategy(cloningStrategy);

        public CalculatorCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;

        }

        public ICalculator GetClone(ICalculator sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            ICalculator newCalculator;
            if (sourceObject is IBeamShearCalculator shearCalculator)
            {
                newCalculator = BeamShearCloneStrategy.GetClone(shearCalculator);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
            return newCalculator;
        }
    }
}
