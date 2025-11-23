using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureCalculatorUpdateCloningStrategy : IUpdateStrategy<ICurvatureCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IHasForceActions> forcesUpdateStrategy;
        private IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;
        private IUpdateStrategy<IHasForceActions> ForcesUpdateStrategy => forcesUpdateStrategy ??= new HasForceActionUpdateCloningStrategy(cloningStrategy);
        private IUpdateStrategy<IHasPrimitives> PrimitivesUpdateStrategy => primitivesUpdateStrategy ??= new HasPrimitivesUpdateCloningStrategy(cloningStrategy);

        public CurvatureCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public CurvatureCalculatorUpdateCloningStrategy(
            ICloningStrategy cloningStrategy,
            IUpdateStrategy<IHasForceActions> forcesUpdateStrategy,
            IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            this.forcesUpdateStrategy = forcesUpdateStrategy;
            this.primitivesUpdateStrategy = primitivesUpdateStrategy;
        }

        public void Update(ICurvatureCalculator targetObject, ICurvatureCalculator sourceObject)
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
