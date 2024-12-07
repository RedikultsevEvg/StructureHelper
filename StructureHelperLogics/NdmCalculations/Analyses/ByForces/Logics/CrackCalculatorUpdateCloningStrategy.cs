using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    /// <summary>
    /// Creates deep copy of crack calculator
    /// </summary>
    public class CrackCalculatorUpdateCloningStrategy : IUpdateStrategy<ICrackCalculator>
    {
        private ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IHasForceActions> forcesUpdateStrategy;
        private IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;

        public CrackCalculatorUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            IUpdateStrategy<IHasForceActions> forcesUpdateStrategy,
            IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesUpdateStrategy = forcesUpdateStrategy;
            this.primitivesUpdateStrategy = primitivesUpdateStrategy;
        }
        public CrackCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy) : this (
            cloningStrategy,
            new HasForceActionUpdateCloningStrategy(cloningStrategy),
            new HasPrimitivesUpdateCloningStrategy(cloningStrategy))
        {
        }

        public void Update(ICrackCalculator targetObject, ICrackCalculator sourceObject)
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
