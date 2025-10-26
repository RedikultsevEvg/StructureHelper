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
        private LinePolygonShapeDTO newItem;

        public LinePolygonToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
            updateStrategy = new LinePolygonShapeUpdateStrategy() { UpdateChildren = false };
            vertexConvertStrategy = new VertexToDTOConvertStrategy(this);
        }

        public override LinePolygonShapeDTO GetNewItem(ILinePolygonShape source)
        {
            ChildClass = this;
            newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            newItem.Clear();
            foreach (var item in source.Vertices)
            {
                VertexDTO newVertex = vertexConvertStrategy.Convert(item);
                newItem.AddVertex(newVertex);
            }
            NewItem = newItem;
            return NewItem;
        }
    }
}
