using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Shapes.Logics
{
    public class PolygonShapeToGraphicPrimitveConvertStrategy : IObjectConvertStrategy<List<IGraphicalPrimitive>, IPolygonShape>
    {
        private PolygonShapeViewModel polygonShapeViewModel;

        public PolygonShapeToGraphicPrimitveConvertStrategy(PolygonShapeViewModel polygonShapeViewModel)
        {
            this.polygonShapeViewModel = polygonShapeViewModel;
        }

        public List<IGraphicalPrimitive> Convert(IPolygonShape source)
        {
            List<IGraphicalPrimitive> primitives = new();
            var polygonPrimitive = new PolygonShapePrimitive(polygonShapeViewModel);
            primitives.Add(polygonPrimitive);
            return primitives;
        }
    }
}
