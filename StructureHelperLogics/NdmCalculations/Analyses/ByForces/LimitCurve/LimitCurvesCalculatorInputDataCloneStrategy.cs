using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve
{
    public class LimitCurvesCalculatorInputDataCloneStrategy : ICloneStrategy<ILimitCurvesCalculatorInputData>
    {
        private IUpdateStrategy<ILimitCurvesCalculatorInputData> updateStrategy;

        public LimitCurvesCalculatorInputDataCloneStrategy(IUpdateStrategy<ILimitCurvesCalculatorInputData> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public LimitCurvesCalculatorInputDataCloneStrategy() : this (new LimitCurvesCalculatorInputDataUpdateStrategy())
        {
            
        }

        public ILimitCurvesCalculatorInputData GetClone(ILimitCurvesCalculatorInputData sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            LimitCurvesCalculatorInputData newItem = new();
            updateStrategy.Update(newItem, sourceObject);
            return newItem;
        }
    }
}
