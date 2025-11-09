using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculatorUpdateStrategy : IParentUpdateStrategy<IValueDiagramCalculator>
    {
        private IUpdateStrategy<IValueDiagramCalculatorInputData> inputDataUpdateStrategy;
        public bool UpdateChildren { get; set; } = true;

        public void Update(IValueDiagramCalculator targetObject, IValueDiagramCalculator sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.ShowTraceData = sourceObject.ShowTraceData;
            if (UpdateChildren == true)
            {
                CheckObject.IsNull(targetObject.InputData, ": target value diagram calculator input data");
                CheckObject.IsNull(sourceObject.InputData, ": source value diagram calculator input data");
                inputDataUpdateStrategy ??= new ValueDiagramCalculatorInputDataUpdateStrategy();
                inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
            }
        }
    }
}
