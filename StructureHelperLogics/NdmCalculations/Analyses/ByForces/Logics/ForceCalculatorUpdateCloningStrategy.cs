using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    /// <summary>
    /// Creates deep copy of force calculator
    /// </summary>
    public class ForceCalculatorUpdateCloningStrategy : IUpdateStrategy<IForceCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private readonly IUpdateStrategy<IHasForceActions> forcesUpdateStrategy;
        private readonly IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;
        public ForceCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy,
            IUpdateStrategy<IHasForceActions> forcesUpdateStrategy,
            IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesUpdateStrategy = forcesUpdateStrategy;
            this.primitivesUpdateStrategy = primitivesUpdateStrategy;
        }

        public ForceCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy) : this (
            cloningStrategy,
            new HasForceActionUpdateCloningStrategy(cloningStrategy),
            new HasPrimitivesUpdateCloningStrategy(cloningStrategy))
        {
        }

        public void Update(IForceCalculator targetObject, IForceCalculator sourceObject)
        {
            CheckObject.IsNull(cloningStrategy);
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            var sourceData = sourceObject.InputData;
            var targetData = targetObject.InputData;
            primitivesUpdateStrategy.Update(targetData, sourceData);
            forcesUpdateStrategy.Update(targetData, sourceData);
        }
    }
}
