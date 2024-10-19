using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculatorInputDataUpdateStrategy : IUpdateStrategy<IForceCalculatorInputData>
    {
        private IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy;
        private IUpdateStrategy<IHasForceCombinations> forceCombinationUpdateStrategy;
        private IUpdateStrategy<IAccuracy> accuracyUpdateStrategy;
        private IUpdateStrategy<ICompressedMember> compressedMemberUpdateStrategy;
        public ForceCalculatorInputDataUpdateStrategy(IUpdateStrategy<IHasPrimitives> primitivesUpdateStrategy,
            IUpdateStrategy<IHasForceCombinations> forceCombinationUpdateStrategy,
            IUpdateStrategy<IAccuracy> accuracyUpdateStrategy,
            IUpdateStrategy<ICompressedMember> compressedMemberUpdateStrategy)
        {
            this.primitivesUpdateStrategy = primitivesUpdateStrategy;
            this.forceCombinationUpdateStrategy = forceCombinationUpdateStrategy;
            this.accuracyUpdateStrategy = accuracyUpdateStrategy;
            this.compressedMemberUpdateStrategy = compressedMemberUpdateStrategy;
        }

        public ForceCalculatorInputDataUpdateStrategy() :
            this(
                new HasPrimitivesUpdateStrategy(),
                new HasForceCombinationUpdateStrategy(),
                new AccuracyUpdateStrategy(),
                new CompressedMemberUpdateStrategy()
                )
        {
        }
        public void Update(IForceCalculatorInputData targetObject, IForceCalculatorInputData sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject, "Force calculator input data");
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Accuracy ??= new Accuracy();
            accuracyUpdateStrategy.Update(targetObject.Accuracy, sourceObject.Accuracy);
            targetObject.CompressedMember ??= new CompressedMember();
            compressedMemberUpdateStrategy.Update(targetObject.CompressedMember, sourceObject.CompressedMember);
            targetObject.LimitStatesList.Clear();
            targetObject.LimitStatesList.AddRange(sourceObject.LimitStatesList);
            targetObject.CalcTermsList.Clear();
            targetObject.CalcTermsList.AddRange(sourceObject.CalcTermsList);
            primitivesUpdateStrategy.Update(targetObject, sourceObject);
            forceCombinationUpdateStrategy.Update(targetObject, sourceObject);
        }
    }
}
