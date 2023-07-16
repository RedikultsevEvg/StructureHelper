using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Sections;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    public class ForceCalculatorUpdateStrategy : IUpdateStrategy<IForceCalculator>
    {
        static readonly AccuracyUpdateStrategy accuracyUpdateStrategy = new();
        static readonly CompressedMemberUpdateStrategy compressedMemberUpdateStrategy = new();
        public void Update(IForceCalculator targetObject, IForceCalculator sourceObject)
        {
            targetObject.Name = sourceObject.Name;
            targetObject.LimitStatesList.Clear();
            targetObject.LimitStatesList.AddRange(sourceObject.LimitStatesList);
            targetObject.CalcTermsList.Clear();
            targetObject.CalcTermsList.AddRange(sourceObject.CalcTermsList);
            accuracyUpdateStrategy.Update(targetObject.Accuracy, sourceObject.Accuracy);
            compressedMemberUpdateStrategy.Update(targetObject.CompressedMember, sourceObject.CompressedMember);
            targetObject.Primitives.Clear();
            targetObject.Primitives.AddRange(sourceObject.Primitives);
            targetObject.ForceActions.Clear();
            targetObject.ForceActions.AddRange(sourceObject.ForceActions);
        }
    }
}
