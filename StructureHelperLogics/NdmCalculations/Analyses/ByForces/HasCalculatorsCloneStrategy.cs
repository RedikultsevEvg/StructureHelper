using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Collection clone strategy for objects which have calculators.
    /// </summary>
    public class HasCalculatorsCloneStrategy : CollectionDeepCloningStrategy<ICalculator>
    {
        public HasCalculatorsCloneStrategy(
            ICloningStrategy cloningStrategy,
            ICloneStrategy<ICalculator> calculatorCloneStrategy)
            : base(cloningStrategy, calculatorCloneStrategy)
        {
        }

        public HasCalculatorsCloneStrategy(ICloningStrategy cloningStrategy)
            : this(cloningStrategy, new CalculatorCloneStrategy(cloningStrategy))
        {
        }

        public void GetClone(
            IHasCalculators target,
            IHasCalculators source,
            CloneMode mode = CloneMode.NewEntity)
        {
            CheckObject.ThrowIfNull(target);
            CheckObject.ThrowIfNull(source);
            GetClone(target.Calculators, source.Calculators, mode);
        }
    }
}
