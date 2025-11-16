using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve
{
    /// <summary>
    /// Creates deep copy of limit curves calculator
    /// </summary>
    public class LimitCurvesCalculatorUpdateCloningStrategy : IUpdateStrategy<ILimitCurvesCalculator>
    {
        private ICloningStrategy cloningStrategy;
        private IUpdateStrategy<ILimitCurvesCalculatorInputData> limitCurvesInputDataUpdateStrategy;

        public LimitCurvesCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy) : this (
            cloningStrategy,
            new LimitCurvesCalculatorInputDataUpdateStrategy())
        {
            this.cloningStrategy = cloningStrategy;
        }

        public LimitCurvesCalculatorUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            IUpdateStrategy<ILimitCurvesCalculatorInputData> limitCurvesInputDataUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.limitCurvesInputDataUpdateStrategy = limitCurvesInputDataUpdateStrategy;
        }

        public void Update(ILimitCurvesCalculator targetObject, ILimitCurvesCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            var targetData = targetObject.InputData;
            limitCurvesInputDataUpdateStrategy.Update(targetData, sourceObject.InputData);
            foreach (var series in targetData.PrimitiveSeries)
            {
                List<INdmPrimitive> collection = UpdatePrimitivesCollection(series);
                series.Collection.AddRange(collection);
            }
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
