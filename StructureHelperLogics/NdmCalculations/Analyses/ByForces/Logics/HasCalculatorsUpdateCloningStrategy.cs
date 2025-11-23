using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Creates deep copy of internal elements of object which has calculators
    /// </summary>
    public class HasCalculatorsUpdateCloningStrategy : IUpdateStrategy<IHasCalculators>
    {
        private ICloningStrategy cloningStrategy;
        private ICalculatorCloningStrategyContainer cloningStrategyContainer;


        public HasCalculatorsUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            ICalculatorCloningStrategyContainer cloningStrategyContainer)
        {
            this.cloningStrategy = cloningStrategy;
            this.cloningStrategyContainer = cloningStrategyContainer;
        }

        public HasCalculatorsUpdateCloningStrategy(ICloningStrategy cloningStrategy) : this(
            cloningStrategy,
            new CalculatorCloningStrategyContainer(cloningStrategy)
            )
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
                var newCalculator = cloningStrategy.Clone(calculator);
                //var newCalculator = calculator.Clone() as ICalculator;
                if (calculator is IForceCalculator forceCalculator)
                {
                    cloningStrategyContainer.ForceCalculatorStrategy.Update(newCalculator as IForceCalculator, forceCalculator);
                }
                else if (calculator is ICrackCalculator crackCalculator)
                {
                    cloningStrategyContainer.CrackCalculatorStrategy.Update(newCalculator as ICrackCalculator, crackCalculator);
                }
                else if (calculator is ILimitCurvesCalculator limitCalculator)
                {
                    cloningStrategyContainer.LimitCurvesCalculatorStrategy.Update(newCalculator as ILimitCurvesCalculator, limitCalculator);
                }
                else if (calculator is IValueDiagramCalculator valueDiagramCalculator)
                {
                    cloningStrategyContainer.ValueDiagramCalculatorStrategy.Update(newCalculator as IValueDiagramCalculator, valueDiagramCalculator);
                }
                else if (calculator is ICurvatureCalculator curvatureCalculator)
                {
                    cloningStrategyContainer.CurvatureCalculatorStrategy.Update(newCalculator as ICurvatureCalculator, curvatureCalculator);
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
