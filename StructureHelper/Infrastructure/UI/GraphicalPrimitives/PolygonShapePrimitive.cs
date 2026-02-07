using StructureHelper.Windows.Shapes;
using StructureHelper.Windows.UserControls;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using PrimitiveVisualProperty = StructureHelperCommon.Models.VisualProperties.PrimitiveVisualProperty;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class PolygonShapePrimitive : IGraphicalPrimitive
    {
        private readonly PolygonShapeViewModel polygonShapeViewModel;
        public string Name => "Polygon";
        public PathGeometry PathGeometry { get; set; }
        public PrimitiveVisualPropertyViewModel VisualProperty { get; } = new(new PrimitiveVisualProperty(Guid.Empty));

        public PolygonShapePrimitive(PolygonShapeViewModel polygonShapeViewModel)
        {
            this.polygonShapeViewModel = polygonShapeViewModel;
            VisualProperty.Color = (Color)ColorConverter.ConvertFromString("DarkGray");
            VisualProperty.FactoredOpacity = 90;

            var polygon = polygonShapeViewModel.GetPolygonShape();
            var logic = new LinePolygonToPathGeometryConvertStrategy();
            try
            {
                PathGeometry = logic.Convert(polygon);
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
