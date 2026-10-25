using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory.Logics;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class HistoryCalculatorUpdateCloningStrategy : IUpdateStrategy<IHistoryCalculator>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IHistoryCalculatorInputData> inputDataCloningStrategy;
        private IUpdateStrategy<IHistoryCalculatorInputData> InputDataCloningStrategy => inputDataCloningStrategy ??= new HistoryCalculatorInputDataUpdateCloningStrategy(cloningStrategy);

        public HistoryCalculatorUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHistoryCalculator targetObject, IHistoryCalculator sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InputDataCloningStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
