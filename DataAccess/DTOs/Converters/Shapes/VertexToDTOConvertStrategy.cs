using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class VertexToDTOConvertStrategy : ConvertStrategy<VertexDTO, IVertex>
    {
        private IUpdateStrategy<IVertex> updateStrategy;
        private IUpdateStrategy<IVertex> UpdateStrategy => updateStrategy ??= new VertexUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;

        public VertexToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override VertexDTO GetNewItem(IVertex source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            pointConvertStrategy = new Point2DToDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            UpdateStrategy.Update(NewItem, source);
            NewItem.Point = pointConvertStrategy.Convert(source.Point);
            return NewItem;
        }
    }
}
