using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculatorInputDataUpdateStrategy : IUpdateStrategy<ForceInputData>
    {
        private IUpdateStrategy<IAccuracy> accuracyUpdateStrategy;
        private IUpdateStrategy<ICompressedMember> compressedMemberUpdateStrategy;
        public ForceCalculatorInputDataUpdateStrategy(IUpdateStrategy<IAccuracy> accuracyUpdateStrategy, IUpdateStrategy<ICompressedMember> compressedMemberUpdateStrategy)
        {
            this.accuracyUpdateStrategy = accuracyUpdateStrategy;
            this.compressedMemberUpdateStrategy = compressedMemberUpdateStrategy;
        }

        public ForceCalculatorInputDataUpdateStrategy() : this(new AccuracyUpdateStrategy(), new CompressedMemberUpdateStrategy()) {        }
        public void Update(ForceInputData targetObject, ForceInputData sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);
            targetObject.Accuracy ??= new Accuracy();
            accuracyUpdateStrategy.Update(targetObject.Accuracy, sourceObject.Accuracy);
            targetObject.CompressedMember ??= new CompressedMember();
            compressedMemberUpdateStrategy.Update(targetObject.CompressedMember, sourceObject.CompressedMember);
            targetObject.LimitStatesList.Clear();
            targetObject.LimitStatesList.AddRange(sourceObject.LimitStatesList);
            targetObject.CalcTermsList.Clear();
            targetObject.CalcTermsList.AddRange(sourceObject.CalcTermsList);
            targetObject.Primitives.Clear();
            targetObject.Primitives.AddRange(sourceObject.Primitives);
            targetObject.ForceActions.Clear();
            targetObject.ForceActions.AddRange(sourceObject.ForceActions);
        }
    }
}
