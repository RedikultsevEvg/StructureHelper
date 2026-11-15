using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Creates deep copy of internal elements of object which has calculators
    /// </summary>
    public class HasCalculatorsUpdateCloningStrategy : IUpdateStrategy<IHasCalculators>
    {
        private ICloningStrategy cloningStrategy;
        private ICloneStrategy<ICalculator> calculatorCloneStrategy;
        private ICloneStrategy<ICalculator> CalculatorCloneStrategy => calculatorCloneStrategy ??= new CalculatorCloningStrategy(cloningStrategy);


        public HasCalculatorsUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            ICloneStrategy<ICalculator> calculatorCloneStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.calculatorCloneStrategy = calculatorCloneStrategy;
        }

        public HasCalculatorsUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
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
                //to do Change to cloning strategy
                var newCalculator = CalculatorCloneStrategy.GetClone(calculator);
                //var newCalculator = calculator.Clone() as ICalculator;
                targetObject.Calculators.Add(newCalculator);
            }
        }
    }
}
