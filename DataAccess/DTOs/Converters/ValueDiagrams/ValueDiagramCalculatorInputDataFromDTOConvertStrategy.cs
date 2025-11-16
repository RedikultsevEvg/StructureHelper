using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class ValueDiagramCalculatorInputDataFromDTOConvertStrategy : ConvertStrategy<ValueDiagramCalculatorInputData, ValueDiagramCalculatorInputDataDTO>
    {
        private IUpdateStrategy<IValueDiagramCalculatorInputData> updateStrategy;
        private IConvertStrategy<ValueDiagramEntity, ValueDiagramEntityDTO> diagramConvertStrategy;
        private IHasPrimitivesProcessLogic primitivesProcessLogic;
        private IHasForceActionsProcessLogic actionsProcessLogic;
        private IConvertStrategy<Accuracy, AccuracyDTO> accuracyConvertStrategy;

        public ValueDiagramCalculatorInputDataFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ValueDiagramCalculatorInputData GetNewItem(ValueDiagramCalculatorInputDataDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            ProcessPrimitives(source);
            ProcessActions(source);
            NewItem.Diagrams.Clear();
            foreach (var diagram in source.Diagrams)
            {
                if (diagram is not ValueDiagramEntityDTO diagramDTO)
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(diagram));
                }
                NewItem.Diagrams.Add(diagramConvertStrategy.Convert(diagramDTO));
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
            updateStrategy ??= new ValueDiagramCalculatorInputDataUpdateStrategy() { UpdateChildren = false };
            diagramConvertStrategy ??= new ValueDiagramEntityFromDTOConvertStrategy(this);
            primitivesProcessLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            actionsProcessLogic ??= new HasForceActionsProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            accuracyConvertStrategy ??= new AccuracyFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        }
    }
}
