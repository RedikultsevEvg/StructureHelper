using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace DataAccess.DTOs
{
    public class ValueDiagramFromDTOConvertStrategy : ConvertStrategy<ValueDiagram, ValueDiagramDTO>
    {
        private IUpdateStrategy<IValueDiagram> updateStrategy;
        private IConvertStrategy<Point2DRange, Point2DRangeDTO> pointConvertStrategy;

        public ValueDiagramFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ValueDiagram GetNewItem(ValueDiagramDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            if (source.Point2DRange is not Point2DRangeDTO point2DRangeDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.Point2DRange));
            }
            NewItem.Point2DRange = pointConvertStrategy.Convert(point2DRangeDTO);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ValueDiagramUpdateStrategy() { UpdateChildren = false };
            pointConvertStrategy ??= new Point2DRangeFromDTOConvertStrategy(this);
        }
    }
}
