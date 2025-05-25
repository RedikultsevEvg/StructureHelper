using StructureHelperCommon.Infrastructures.Interfaces;

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

        public ILimitCurvesCalculatorInputData GetClone(ILimitCurvesCalculatorInputData sourceObject)
        {
            LimitCurvesCalculatorInputData newItem = new();
            updateStrategy.Update(newItem, sourceObject);
            return newItem;
        }
    }
}
