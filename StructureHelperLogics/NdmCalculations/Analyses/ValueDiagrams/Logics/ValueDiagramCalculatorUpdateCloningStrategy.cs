using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculatorUpdateCloningStrategy : IUpdateStrategy<IValueDiagramCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IHasForceActions> forcesUpdateStrategy;
        private IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;
        private IUpdateStrategy<IHasForceActions> ForcesUpdateStrategy => forcesUpdateStrategy ??= new HasForceActionUpdateCloningStrategy(cloningStrategy);
        private IUpdateStrategy<IHasPrimitives> PrimitivesUpdateStrategy => primitivesUpdateStrategy ??= new HasPrimitivesUpdateCloningStrategy(cloningStrategy);

        public ValueDiagramCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy)
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
            PrimitivesUpdateStrategy.Update(targetData, sourceData);
            ForcesUpdateStrategy.Update(targetData, sourceData);
        }
    }
}
