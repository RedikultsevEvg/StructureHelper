using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    internal class LimitCurvesCalculatorUpdateStrategy : IUpdateStrategy<ILimitCurvesCalculator>
    {
        LimitCurveInputDataUpdateStrategy inputDataUpdateStrategy => new();
        public void Update(ILimitCurvesCalculator targetObject, ILimitCurvesCalculator sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.ActionToOutputResults = sourceObject.ActionToOutputResults;
            inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
