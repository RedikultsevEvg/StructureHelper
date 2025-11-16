using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class Point2DRangeFromDTOConvertStrategy : ConvertStrategy<Point2DRange, Point2DRangeDTO>
    {
        private IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy;

        public Point2DRangeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override Point2DRange GetNewItem(Point2DRangeDTO source)
        {
            ChildClass = this;
            CheckSourceObject(source);
            NewItem = new(source.Id);
            InitializeStrategies();
            if (source.StartPoint is not Point2DDTO startPoint)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.StartPoint) + ": start point");
            }
            if (source.EndPoint is not Point2DDTO endPoint)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.StartPoint) + ": end point");
            }
            NewItem.StartPoint = pointConvertStrategy.Convert(startPoint);
            NewItem.EndPoint = pointConvertStrategy.Convert(endPoint);
            return NewItem;
        }

        private static void CheckSourceObject(Point2DRangeDTO source)
        {
            CheckObject.IsNull(source, ": point range source object");
            CheckObject.IsNull(source.StartPoint + ": point range start point");
            CheckObject.IsNull(source.EndPoint + ": point range end point");
        }

        private void InitializeStrategies()
        {
            pointConvertStrategy ??= new Point2DFromDTOConvertStrategy(this);
        }
    }
}
