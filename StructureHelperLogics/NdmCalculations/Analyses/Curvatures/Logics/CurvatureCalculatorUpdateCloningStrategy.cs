using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorUpdateCloningStrategy : IUpdateStrategy<ICurvatureCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly ICollectionCloneStrategy<IForceAction> forcesCloneStrategy;
        private readonly ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy;

        public CurvatureCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy)
            : this(
                cloningStrategy,
                new HasForceActionsCloneStrategy(cloningStrategy),
                new HasPrimitivesCloneStrategy(cloningStrategy))
        {
        }

        public CurvatureCalculatorUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            ICollectionCloneStrategy<IForceAction> forcesCloneStrategy,
            ICollectionCloneStrategy<INdmPrimitive> primitivesCloneStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesCloneStrategy = forcesCloneStrategy;
            this.primitivesCloneStrategy = primitivesCloneStrategy;
        }

        public void Update(ICurvatureCalculator targetObject, ICurvatureCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            var sourceData = sourceObject.InputData;
            var targetData = targetObject.InputData;
            primitivesCloneStrategy.GetClone(targetData.Primitives, sourceData.Primitives, CloneMode.NewEntity);
            forcesCloneStrategy.GetClone(targetData.ForceActions, sourceData.ForceActions, CloneMode.NewEntity);
        }
    }
}
