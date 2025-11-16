using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class Point2DRangeToDTOConvertStrategy : ConvertStrategy<Point2DRangeDTO, IPoint2DRange>
    {
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy = new Point2DToDTOConvertStrategy();

        public Point2DRangeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override Point2DRangeDTO GetNewItem(IPoint2DRange source)
        {
            ChildClass = this;
            NewItem = new(source.Id)
            {
                StartPoint = pointConvertStrategy.Convert(source.StartPoint),
                EndPoint = pointConvertStrategy.Convert(source.EndPoint)
            };
            return NewItem;
        }
    }
}
