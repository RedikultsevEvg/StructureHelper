using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace DataAccess.DTOs
{
    public class ValueDiagramToDTOConvertStrategy : ConvertStrategy<ValueDiagramDTO, IValueDiagram>
    {
        private IUpdateStrategy<IValueDiagram> updateStrategy;
        private IConvertStrategy<Point2DRangeDTO, IPoint2DRange> pointConvertStrategy;

        public ValueDiagramToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ValueDiagramDTO GetNewItem(IValueDiagram source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            NewItem.Point2DRange = pointConvertStrategy.Convert(source.Point2DRange);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ValueDiagramUpdateStrategy() { UpdateChildren = false };
            pointConvertStrategy ??= new Point2DRangeToDTOConvertStrategy(this);
        }
    }
}
