using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class VertexFtomDTOConvertStrategy : ConvertStrategy<IVertex, IVertex>
    {
        private IUpdateStrategy<IVertex> updateStrategy;
        private IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy;
        private Vertex newItem;

        public VertexFtomDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
            updateStrategy = new VertexUpdateStrategy() { UpdateChildren = false };
            pointConvertStrategy = new Point2DFromDTOConvertStrategy(this);
        }

        public override IVertex GetNewItem(IVertex source)
        {
            ChildClass = this;
            if (source is not VertexDTO sourceDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source) + ": source vertex is not DTO object");
            }
            newItem = new(sourceDTO.Id);
            updateStrategy.Update(newItem, sourceDTO);
            newItem.Point = pointConvertStrategy.Convert(sourceDTO.Point as Point2DDTO);
            NewItem = newItem;
            return NewItem;
        }
    }
}
