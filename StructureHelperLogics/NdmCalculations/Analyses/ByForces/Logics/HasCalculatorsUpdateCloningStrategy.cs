using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
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
        private IUpdateStrategy<IHasForceActions> forcesUpdateStrategy;
        private IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;
        private IUpdateStrategy<ILimitCurvesCalculatorInputData> limitCurvesInputDataUpdateStrategy;

        public HasCalculatorsUpdateCloningStrategy(ICloningStrategy cloningStrategy) : this(
            cloningStrategy,
            new HasForceActionUpdateCloningStrategy(cloningStrategy),
            new HasPrimitivesUpdateCloningStrategy(cloningStrategy),
            new LimitCurvesCalculatorInputDataUpdateStrategy()
            )
        {
        }

        public HasCalculatorsUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            IUpdateStrategy<IHasForceActions> forcesUpdateStrategy,
            IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy,
            IUpdateStrategy<ILimitCurvesCalculatorInputData> limitCurvesInputDataUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesUpdateStrategy = forcesUpdateStrategy;
            this.primitivesUpdateStrategy = primitivesUpdateStrategy;
            this.limitCurvesInputDataUpdateStrategy = limitCurvesInputDataUpdateStrategy;
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
                var newCalculator = cloningStrategy.Clone(calculator);
                if (calculator is IForceCalculator forceCalculator)
                {
                    ProcessForceCalculator(newCalculator, forceCalculator);
                }
                else if (calculator is CrackCalculator crackCalculator)
                {
                    ProcessCrackCalculator(newCalculator, crackCalculator);
                }
                else if (calculator is ILimitCurvesCalculator limitCalculator)
                {
                    ProcessLimitCurvesCalculator(newCalculator, limitCalculator);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(calculator));
                }
                targetObject.Calculators.Add(newCalculator);
            }
        }

        private void ProcessLimitCurvesCalculator(ICalculator newCalculator, ILimitCurvesCalculator limitCalculator)
        {
            var sourceData = limitCalculator.InputData;
            var targetData = ((ILimitCurvesCalculator)newCalculator).InputData;
            limitCurvesInputDataUpdateStrategy.Update(targetData, sourceData);
            foreach (var series in targetData.PrimitiveSeries)
            {
                List<INdmPrimitive> collection = UpdatePrimitivesCollection(series);
                series.Collection.AddRange(collection);
            }
        }

        private void ProcessCrackCalculator(ICalculator newCalculator, CrackCalculator crackCalculator)
        {
            var sourceData = crackCalculator.InputData;
            var targetData = ((ICrackCalculator)newCalculator).InputData;
            primitivesUpdateStrategy.Update(targetData, sourceData);
            forcesUpdateStrategy.Update(targetData, sourceData);
        }

        private void ProcessForceCalculator(ICalculator newCalculator, IForceCalculator forceCalculator)
        {
            var sourceData = forceCalculator.InputData;
            var targetData = ((IForceCalculator)newCalculator).InputData;
            primitivesUpdateStrategy.Update(targetData, sourceData);
            forcesUpdateStrategy.Update(targetData, sourceData);
        }

        private List<INdmPrimitive> UpdatePrimitivesCollection(NamedCollection<INdmPrimitive> series)
        {
            List<INdmPrimitive> collection = new();
            foreach (var item in series.Collection)
            {
                var newItem = cloningStrategy.Clone(item);
                collection.Add(newItem);
            }
            series.Collection.Clear();
            return collection;
        }
    }
}
