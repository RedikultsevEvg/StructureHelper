using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class LinePolygonFromDTOConvertStrategy : ConvertStrategy<ILinePolygonShape, ILinePolygonShape>
    {
        private IUpdateStrategy<ILinePolygonShape> updateStrategy;
        private IConvertStrategy<IVertex, IVertex> vertexConvertStrategy;
        private LinePolygonShape newItem;

        public LinePolygonFromDTOConvertStrategy(IUpdateStrategy<ILinePolygonShape> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public LinePolygonFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
            updateStrategy = new LinePolygonShapeUpdateStrategy() { UpdateChildren = false };
            vertexConvertStrategy = new VertexFtomDTOConvertStrategy(this);
        }

        public override ILinePolygonShape GetNewItem(ILinePolygonShape source)
        {
            ChildClass = this;
            if (source is not LinePolygonShapeDTO sourceDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source) + ": source line polygon is not DTO object");
            }
            newItem = new(sourceDTO.Id);
            updateStrategy.Update(newItem, sourceDTO);
            newItem.Clear();
            foreach (var item in source.Vertices)
            {
                IVertex newVertex = vertexConvertStrategy.Convert(item);
                newItem.AddVertex(newVertex);
            }
            NewItem = newItem;
            return NewItem;
        }
    }
}
