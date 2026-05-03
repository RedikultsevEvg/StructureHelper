using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System.Collections.Generic;

namespace StructureHelper.Windows.Shapes.Logics
{
    public class PolygonShapeToGraphicPrimitveConvertStrategy : IObjectConvertStrategy<List<IGraphicalPrimitive>, ILinePolygonShape>
    {
        private PolygonShapeViewModel polygonShapeViewModel;

        public PolygonShapeToGraphicPrimitveConvertStrategy(PolygonShapeViewModel polygonShapeViewModel)
        {
            this.polygonShapeViewModel = polygonShapeViewModel;
        }

        public List<IGraphicalPrimitive> Convert(ILinePolygonShape source)
        {
            List<IGraphicalPrimitive> primitives = new();
            var polygonPrimitive = new PolygonShapePrimitive(polygonShapeViewModel);
            primitives.Add(polygonPrimitive);
            return primitives;
        }
    }
}
