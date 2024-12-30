using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
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
        private IUpdateStrategy<IForceCalculator> forceCalculatorUpdateStrategy;
        private IUpdateStrategy<ICrackCalculator> crackCalculatorUpdateStrategy;
        private IUpdateStrategy<ILimitCurvesCalculator> limitCurvesCalculatorUpdateStrategy;


        public HasCalculatorsUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            IUpdateStrategy<IForceCalculator> forceCalculatorUpdateStrategy,
            IUpdateStrategy<ICrackCalculator> crackCalculatorUpdateStrategy,
            IUpdateStrategy<ILimitCurvesCalculator> limitCurvesCalculatorUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forceCalculatorUpdateStrategy = forceCalculatorUpdateStrategy;
            this.crackCalculatorUpdateStrategy = crackCalculatorUpdateStrategy;
            this.limitCurvesCalculatorUpdateStrategy = limitCurvesCalculatorUpdateStrategy;
        }

        public HasCalculatorsUpdateCloningStrategy(ICloningStrategy cloningStrategy) : this(
            cloningStrategy,
            new ForceCalculatorUpdateCloningStrategy(cloningStrategy),
            new CrackCalculatorUpdateCloningStrategy(cloningStrategy),
            new LimitCurvesCalculatorUpdateCloningStrategy(cloningStrategy)
            )
        {
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
                //to do Change to cloning strategy
                var newCalculator = cloningStrategy.Clone(calculator);
                //var newCalculator = calculator.Clone() as ICalculator;
                if (calculator is IForceCalculator forceCalculator)
                {
                    forceCalculatorUpdateStrategy.Update(newCalculator as IForceCalculator, forceCalculator);
                }
                else if (calculator is ICrackCalculator crackCalculator)
                {
                    crackCalculatorUpdateStrategy.Update(newCalculator as ICrackCalculator, crackCalculator);
                }
                else if (calculator is ILimitCurvesCalculator limitCalculator)
                {
                    limitCurvesCalculatorUpdateStrategy.Update(newCalculator as ILimitCurvesCalculator, limitCalculator);
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
