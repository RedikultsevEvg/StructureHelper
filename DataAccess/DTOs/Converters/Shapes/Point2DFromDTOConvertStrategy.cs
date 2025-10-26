using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class Point2DFromDTOConvertStrategy : ConvertStrategy<Point2D, Point2DDTO>
    {
        private IUpdateStrategy<IPoint2D> updateStrategy;

        public Point2DFromDTOConvertStrategy(IUpdateStrategy<IPoint2D> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public Point2DFromDTOConvertStrategy() : this (new Point2DUpdateStrategy())
        {
            
        }

        public Point2DFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
            updateStrategy = new Point2DUpdateStrategy();
        }

        public override Point2D GetNewItem(Point2DDTO source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
