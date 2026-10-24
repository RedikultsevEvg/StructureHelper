using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class ConstructionStageInputDataUpdateCloningStrategy : IUpdateStrategy<IConstructionStageInputData>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;
        private IUpdateStrategy<IHasPrimitives> PrimitivesUpdateStrategy => primitivesUpdateStrategy ??= new HasPrimitivesUpdateCloningStrategy(cloningStrategy);

        public void Update(IConstructionStageInputData targetObject, IConstructionStageInputData sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ForceAction = cloningStrategy.Clone(sourceObject.ForceAction);
            PrimitivesUpdateStrategy.Update(targetObject, sourceObject);
        }
    }
}
