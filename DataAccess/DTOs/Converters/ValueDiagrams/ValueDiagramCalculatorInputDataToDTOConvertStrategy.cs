using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class ValueDiagramCalculatorInputDataToDTOConvertStrategy : ConvertStrategy<ValueDiagramCalculatorInputDataDTO, IValueDiagramCalculatorInputData>
    {
        private IUpdateStrategy<IValueDiagramCalculatorInputData> updateStrategy;
        private IConvertStrategy<ValueDiagramEntityDTO, IValueDiagramEntity> diagramConvertStrategy;
        private IConvertStrategy<ValueDiagramEntityDTO, IValueDiagramEntity> DiagramConvertStrategy => diagramConvertStrategy ??= new ValueDiagramEntityToDTOConvertStrategy(this);
        private IProcessLogic<IHasForcesAndPrimitives> actionsProcessLogic;
        private IUpdateStrategy<IValueDiagramCalculatorInputData> UpdateStrategy => updateStrategy ??= new ValueDiagramCalculatorInputDataUpdateStrategy() { UpdateChildren = false };
        private IProcessLogic<IHasForcesAndPrimitives> ActionsProcessLogic => actionsProcessLogic ??= new HasForcesAndPrimitivesProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };

        public ValueDiagramCalculatorInputDataToDTOConvertStrategy() : base()
        {
        }

        public ValueDiagramCalculatorInputDataToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ValueDiagramCalculatorInputDataDTO GetNewItem(IValueDiagramCalculatorInputData source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            ProcessActions(source);
            NewItem.Diagrams.Clear();
            foreach (var diagram in source.Diagrams)
            {
                NewItem.Diagrams.Add(DiagramConvertStrategy.Convert(diagram));
            }
            return NewItem;
        }

        private void ProcessActions(IHasForcesAndPrimitives source)
        {
            ActionsProcessLogic.Source = source;
            ActionsProcessLogic.Target = NewItem;
            ActionsProcessLogic.Process();
        }
    }
}
