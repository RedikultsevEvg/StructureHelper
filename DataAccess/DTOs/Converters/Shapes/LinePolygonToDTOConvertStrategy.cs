using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class LinePolygonToDTOConvertStrategy : ConvertStrategy<LinePolygonShapeDTO, ILinePolygonShape>
    {
        private IUpdateStrategy<ILinePolygonShape> updateStrategy;
        private IConvertStrategy<VertexDTO, IVertex> vertexConvertStrategy;

        public LinePolygonToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override LinePolygonShapeDTO GetNewItem(ILinePolygonShape source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            updateStrategy = new LinePolygonShapeUpdateStrategy() { UpdateChildren = false };
            vertexConvertStrategy = new VertexToDTOConvertStrategy(this);
            updateStrategy.Update(NewItem, source);
            NewItem.Clear();
            foreach (var item in source.Vertices)
            {
                VertexDTO newVertex = vertexConvertStrategy.Convert(item);
                NewItem.AddVertex(newVertex);
            }
            return NewItem;
        }
    }
}
