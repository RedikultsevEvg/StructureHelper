using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculatorUpdateCloningStrategy : IUpdateStrategy<IValueDiagramCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly IUpdateStrategy<IHasForceActions> forcesUpdateStrategy;
        private readonly IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;

        public ValueDiagramCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy) : this(
            cloningStrategy,
            new HasForceActionUpdateCloningStrategy(cloningStrategy),
            new HasPrimitivesUpdateCloningStrategy(cloningStrategy))
        {
            this.cloningStrategy = cloningStrategy;
        }


        public ValueDiagramCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy,
            IUpdateStrategy<IHasForceActions> forcesUpdateStrategy,
            IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesUpdateStrategy = forcesUpdateStrategy;
            this.primitivesUpdateStrategy = primitivesUpdateStrategy;
        }

        public void Update(IValueDiagramCalculator targetObject, IValueDiagramCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            var sourceData = sourceObject.InputData;
            var targetData = targetObject.InputData;
            primitivesUpdateStrategy.Update(targetData, sourceData);
            forcesUpdateStrategy.Update(targetData, sourceData);
        }
    }
}
