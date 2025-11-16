using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackCalculatorUpdateStrategy : IUpdateStrategy<ICrackCalculator>
    {
        private IUpdateStrategy<ICrackCalculatorInputData> inputDataUpdateStrategy;

        public CrackCalculatorUpdateStrategy(IUpdateStrategy<ICrackCalculatorInputData> inputDataUpdateStrategy)
        {
            this.inputDataUpdateStrategy = inputDataUpdateStrategy;
        }
        public CrackCalculatorUpdateStrategy() : this(new CrackInputDataUpdateStrategy()) { }
        public void Update(ICrackCalculator targetObject, ICrackCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.ShowTraceData = sourceObject.ShowTraceData;
            targetObject.InputData ??= new CrackCalculatorInputData();
            inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
