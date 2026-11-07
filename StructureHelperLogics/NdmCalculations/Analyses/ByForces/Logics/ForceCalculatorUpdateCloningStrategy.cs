using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    /// <summary>
    /// Creates deep copy of force calculator
    /// </summary>
    public class ForceCalculatorUpdateCloningStrategy : IUpdateStrategy<IForceCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICollectionCloneStrategy<IForceAction> forcesCloneStrategy;
        private readonly ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy;

        public ForceCalculatorUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            ICollectionCloneStrategy<IForceAction> forcesCloneStrategy,
            ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesCloneStrategy = forcesCloneStrategy;
            this.primitivesCloneStrategy = primitivesCloneStrategy;
        }

        public ForceCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy)
            : this(
                cloningStrategy,
                new HasForceActionsCloneStrategy(cloningStrategy),
                new HasPrimitivesCloneStrategy(cloningStrategy))
        {
        }

        public void Update(IForceCalculator targetObject, IForceCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            var sourceData = sourceObject.InputData;
            var targetData = targetObject.InputData;
            primitivesCloneStrategy.GetClone(targetData.Primitives, sourceData.Primitives, CloningMode.NewEntity);
            forcesCloneStrategy.GetClone(targetData.ForceActions, sourceData.ForceActions, CloningMode.NewEntity);
        }
    }
}
