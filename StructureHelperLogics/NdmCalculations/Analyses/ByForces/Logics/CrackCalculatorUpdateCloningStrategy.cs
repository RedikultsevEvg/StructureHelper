using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    /// <summary>
    /// Creates deep copy of crack calculator
    /// </summary>
    public class CrackCalculatorUpdateCloningStrategy : IUpdateStrategy<ICrackCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICollectionCloneStrategy<IForceAction> forcesCloneStrategy;
        private readonly ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy;

        public CrackCalculatorUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            ICollectionCloneStrategy<IForceAction> forcesCloneStrategy,
            ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesCloneStrategy = forcesCloneStrategy;
            this.primitivesCloneStrategy = primitivesCloneStrategy;
        }

        public CrackCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy)
            : this(
                cloningStrategy,
                new HasForceActionsCloneStrategy(cloningStrategy),
                new HasPrimitivesCloneStrategy(cloningStrategy))
        {
        }

        public void Update(ICrackCalculator targetObject, ICrackCalculator sourceObject)
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
