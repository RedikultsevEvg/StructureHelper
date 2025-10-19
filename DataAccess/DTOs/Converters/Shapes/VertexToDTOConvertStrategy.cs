using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class VertexToDTOConvertStrategy : ConvertStrategy<VertexDTO, IVertex>
    {
        private IUpdateStrategy<IVertex> updateStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;

        public VertexToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override VertexDTO GetNewItem(IVertex source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            updateStrategy = new VertexUpdateStrategy() { UpdateChildren = false };
            pointConvertStrategy = new Point2DToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            updateStrategy.Update(NewItem, source);
            NewItem.Point = pointConvertStrategy.Convert(source.Point);
            return NewItem;
        }
    }
}
