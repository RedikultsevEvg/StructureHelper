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
        private IHasPrimitivesProcessLogic primitivesProcessLogic;
        private IHasForceActionsProcessLogic actionsProcessLogic;

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
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            ProcessPrimitives(source);
            ProcessActions(source);
            NewItem.Digrams.Clear();
            foreach (var diagram in source.Digrams)
            {
                NewItem.Digrams.Add(diagramConvertStrategy.Convert(diagram));
            }
            return NewItem;
        }

        private void ProcessPrimitives(IHasPrimitives source)
        {
            primitivesProcessLogic.Source = source;
            primitivesProcessLogic.Target = NewItem;
            primitivesProcessLogic.ReferenceDictionary = ReferenceDictionary;
            primitivesProcessLogic.TraceLogger = TraceLogger;
            primitivesProcessLogic.Process();
        }
        private void ProcessActions(IHasForceActions source)
        {
            actionsProcessLogic.Source = source;
            actionsProcessLogic.Target = NewItem;
            actionsProcessLogic.ReferenceDictionary = ReferenceDictionary;
            actionsProcessLogic.TraceLogger = TraceLogger;
            actionsProcessLogic.Process();
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ValueDiagramCalculatorInputDataUpdateStrategy() { UpdateChildren = false};
            diagramConvertStrategy ??= new ValueDiagramEntityToDTOConvertStrategy(this);
            primitivesProcessLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            actionsProcessLogic ??= new HasForceActionsProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        }
    }
}
