using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace DataAccess.DTOs
{
    public class ValueDiagramEntityToDTOConvertStrategy : ConvertStrategy<ValueDiagramEntityDTO, IValueDiagramEntity>
    {
        private IUpdateStrategy<IValueDiagramEntity> updateStrategy;
        private IConvertStrategy<ValueDiagramDTO, IValueDiagram> diagramConvertStrategy;

        public ValueDiagramEntityToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ValueDiagramEntityDTO GetNewItem(IValueDiagramEntity source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            NewItem.ValueDiagram = diagramConvertStrategy.Convert(source.ValueDiagram);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ValueDiagramEntityUpdateStrategy() { UpdateChildren = false };
            diagramConvertStrategy ??= new ValueDiagramToDTOConvertStrategy(this);
        }
    }
}
