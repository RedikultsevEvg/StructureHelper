using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Media;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class ShapeViewPrimitive : PrimitiveBase
    {
        IShapeNDMPrimitive shapeNDMPrimitive;

        public PathGeometry PathGeometry { get; set; }

        public ShapeViewPrimitive(IShapeNDMPrimitive shapeNDMPrimitive) : base(shapeNDMPrimitive)
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
            if (shape is not IPolygonShape polygon)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(shape));
            }
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
            return new(DeltaX + shapeNDMPrimitive.Center.X + helperPoint.X, DeltaY - shapeNDMPrimitive.Center.Y - helperPoint.Y);
        }
    }
}
