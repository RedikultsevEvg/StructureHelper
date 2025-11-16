using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace DataAccess.DTOs
{
    public class ValueDiagramEntityFromDTOConvertStrategy : ConvertStrategy<ValueDiagramEntity, ValueDiagramEntityDTO>
    {
        private IUpdateStrategy<IValueDiagramEntity> updateStrategy;
        private IConvertStrategy<ValueDiagram, ValueDiagramDTO> diagramConvertStrategy;

        public ValueDiagramEntityFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ValueDiagramEntity GetNewItem(ValueDiagramEntityDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            if (source.ValueDiagram is not ValueDiagramDTO diagramDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.ValueDiagram));
            }
            NewItem.ValueDiagram = diagramConvertStrategy.Convert(diagramDTO);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ValueDiagramEntityUpdateStrategy() { UpdateChildren = false};
            diagramConvertStrategy ??= new ValueDiagramFromDTOConvertStrategy(this);
        }
    }
}
