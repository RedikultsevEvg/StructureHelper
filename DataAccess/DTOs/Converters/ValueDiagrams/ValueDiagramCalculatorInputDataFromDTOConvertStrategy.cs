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
        private IProcessLogic<IHasForcesAndPrimitives> forcesAndPrimitivesLogic;
        private IConvertStrategy<Accuracy, AccuracyDTO> accuracyConvertStrategy;
        private IUpdateStrategy<IValueDiagramCalculatorInputData> UpdateStrategy => updateStrategy ??= new ValueDiagramCalculatorInputDataUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<ValueDiagramEntity, ValueDiagramEntityDTO> DiagramConvertStrategy => diagramConvertStrategy ??= new ValueDiagramEntityFromDTOConvertStrategy(this);
        private IProcessLogic<IHasForcesAndPrimitives> ForcesAndPrimitivesLogic => forcesAndPrimitivesLogic ??= new HasForcesAndPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        private IConvertStrategy<Accuracy, AccuracyDTO> AccuracyConvertStrategy => accuracyConvertStrategy ??= new AccuracyFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };

        public ValueDiagramCalculatorInputDataFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public ValueDiagramCalculatorInputDataFromDTOConvertStrategy(
            IUpdateStrategy<IValueDiagramCalculatorInputData> updateStrategy,
            IConvertStrategy<ValueDiagramEntity, ValueDiagramEntityDTO> diagramConvertStrategy,
            IProcessLogic<IHasForcesAndPrimitives> forcesAndPrimitivesLogic,
            IConvertStrategy<Accuracy, AccuracyDTO> accuracyConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.diagramConvertStrategy = diagramConvertStrategy;
            this.forcesAndPrimitivesLogic = forcesAndPrimitivesLogic;
            this.accuracyConvertStrategy = accuracyConvertStrategy;
        }

        public override ValueDiagramCalculatorInputData GetNewItem(ValueDiagramCalculatorInputDataDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            ProcessForcesAndPrimitives(source);
            NewItem.Diagrams.Clear();
            foreach (var diagram in source.Diagrams)
            {
                if (diagram is not ValueDiagramEntityDTO diagramDTO)
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(diagram));
                }
                NewItem.Diagrams.Add(DiagramConvertStrategy.Convert(diagramDTO));
            }
            return NewItem;
        }

        private void ProcessForcesAndPrimitives(IHasForcesAndPrimitives source)
        {
            ForcesAndPrimitivesLogic.Source = source;
            ForcesAndPrimitivesLogic.Target = NewItem;
            ForcesAndPrimitivesLogic.Process();
        }
    }
}
