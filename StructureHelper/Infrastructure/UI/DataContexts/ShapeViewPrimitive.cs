using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Linq;
using System.Windows.Media;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class ShapeViewPrimitive : PrimitiveBase
    {
        IShapeNdmPrimitive shapeNDMPrimitive;

        public PathGeometry PathGeometry { get; set; }

        public ShapeViewPrimitive(IShapeNdmPrimitive shapeNDMPrimitive) : base(shapeNDMPrimitive)
        {
            this.shapeNDMPrimitive = shapeNDMPrimitive;
            DivisionViewModel = new HasDivisionViewModel(this.shapeNDMPrimitive.DivisionSize);
            UpdatePath();
        }

        public override void Refresh()
        {
            UpdatePath();
            OnPropertyChanged(nameof(CenterX));
            OnPropertyChanged(nameof(CenterY));
            OnPropertyChanged(nameof(PathGeometry));
            base.Refresh();
        }

        private void UpdatePath()
        {
            var shape = shapeNDMPrimitive.Shape;
            if (shape is ILinePolygonShape polygon)
            {
                if (polygon.Vertices.Count == 0) return;
                GetPathByPolygon(polygon);
            }
            else if (shape is IVerticalTShape tShape)
            {
                var strategy = new VerticalTShapeToPolygonConvertStrategy();
                var newPolygon = strategy.Convert(tShape);
                GetPathByPolygon(newPolygon);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(shape));
            }

        }

        private void GetPathByPolygon(ILinePolygonShape polygon)
        {
            var points = polygon.Vertices.Select(x => x.Point).ToList();
            IPoint2D StartPoint = points[0];
            System.Windows.Point systemPoint = GetSystemPoint(StartPoint);
            var figure = new PathFigure
            {
                StartPoint = systemPoint
            };
            for (int i = 1; i < points.Count; i++)
                figure.Segments.Add(new LineSegment(GetSystemPoint(points[i]), true));
            figure.IsClosed = true;
            PathGeometry = new PathGeometry(new[] { figure });
        }

        private System.Windows.Point GetSystemPoint(IPoint2D helperPoint)
        {
            return new(DeltaX + helperPoint.X, DeltaY - helperPoint.Y);
        }
    }
}
