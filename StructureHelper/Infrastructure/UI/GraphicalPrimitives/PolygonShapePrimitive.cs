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
            var points = polygon.Vertices.Select(x => x.Point).ToList();
            if (points.Count == 0) return;

            IPoint2D StartPoint = points[0];
            System.Windows.Point systemPoint = GetSystemPoint(StartPoint);
            var figure = new PathFigure { StartPoint = systemPoint };
            for (int i = 1; i < points.Count; i++)
                figure.Segments.Add(new LineSegment(GetSystemPoint(points[i]), true));
            figure.IsClosed = true;
            PathGeometry = new PathGeometry(new[] { figure });
        }

        private System.Windows.Point GetSystemPoint(IPoint2D helperPoint)
        {
            return new(helperPoint.X, helperPoint.Y);
        }
    }
}
