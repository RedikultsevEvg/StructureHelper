using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
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
            : this(cloningStrategy, new CalculatorCloneStrategy())
        {
        }

        public void GetClone(
            IHasCalculators target,
            IHasCalculators source,
            CloningMode mode = CloningMode.NewEntity)
        {
            CheckObject.ThrowIfNull(target);
            CheckObject.ThrowIfNull(source);
            GetClone(target.Calculators, source.Calculators, mode);
        }
    }
}
