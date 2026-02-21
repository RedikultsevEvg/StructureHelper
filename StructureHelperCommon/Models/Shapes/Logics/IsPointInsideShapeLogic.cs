using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace StructureHelperCommon.Models.Shapes
{
    public class IsPointInsideShapeLogic : IsPontInsideShapeLogic
    {
        private ICenterShape centerShape;
        private IPoint2D point;

        public double Gap { get; set; } = 1e-10;

        public bool IsPontInside(IPoint2D point, ICenterShape centerShape)
        {
            this.centerShape = centerShape;
            this.point = point;
            if (centerShape.Shape is IRectangleShape) { return ProcessRectangle(); }
            else if (centerShape.Shape is ICircleShape) { return ProcessCircle(); }
            else if (centerShape.Shape is IRingShape) { return ProcessRing(); }
            else if (centerShape.Shape is ILinePolygonShape) { return ProcessLinePolygon((ILinePolygonShape)centerShape.Shape); }
            else if (centerShape.Shape is IVerticalDoubleTShape) { return ProcessVerticalDoubleTShape(); }
            else if (centerShape.Shape is IVerticalTShape) { return ProcessVerticalTShape(); }
            else if (centerShape.Shape is ITrapezoidShape) { return ProcessTrapezoidShape(); }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(centerShape) + ": shape for calculation if point inside is uknown");
            }
        }

        private bool ProcessTrapezoidShape()
        {
            var strategy = new TrapezoidShapeToPolygonConvertStrategy();
            var shape = (ITrapezoidShape)centerShape.Shape;
            var newPolygon = strategy.Convert(shape);
            return ProcessLinePolygon(newPolygon);
        }

        private bool ProcessVerticalDoubleTShape()
        {
            var strategy = new VerticalDoubleTShapeToPolygonConvertStrategy();
            var shape = (IVerticalDoubleTShape)centerShape.Shape;
            var newPolygon = strategy.Convert(shape);
            return ProcessLinePolygon(newPolygon);
        }

        private bool ProcessVerticalTShape()
        {
            var strategy = new VerticalTShapeToPolygonConvertStrategy();
            var tShape = (IVerticalTShape)centerShape.Shape;
            var newPolygon = strategy.Convert(tShape);
            return ProcessLinePolygon(newPolygon);
        }

        private bool ProcessLinePolygon(ILinePolygonShape polygon)
        {
            var newShape = PolygonGeometryUtils.GetTransfromedPolygon(polygon, centerShape.Center.X, centerShape.Center.Y);
            newShape.IsClosed = true;
            var calculator = new PolygonCalculator();
            return calculator.ContainsPoint(newShape, point);
        }

        private bool ProcessRing()
        {
            double distance = GetDistance();
            var ringShape = (IRingShape)centerShape.Shape;
            if (
                distance < ringShape.OuterRadius + Gap
                &&
                distance > ringShape.InnerRadius - Gap
                )
            {
                return true;
            }
            return false;
        }

        private bool ProcessCircle()
        {
            double distance = GetDistance();
            var circleShape = (ICircleShape)centerShape;
            if (distance < circleShape.Diameter / 2 + Gap) {return true;}
            return false;
        }

        private bool ProcessRectangle()
        {
            var (dx, dy) = GetDeltas();
            var rectangle = (IRectangleShape)centerShape.Shape;
            if (
                dx <= rectangle.Width / 2 + Gap
                &&
                dy <= rectangle.Height / 2 + Gap
                )
            {
                return true;
            }
            return false;
        }

        private (double dx, double dy) GetDeltas()
        {
            double dx = Math.Abs(point.X - centerShape.Center.X);
            double dy = Math.Abs(point.Y - centerShape.Center.Y);
            return new (dx, dy);
        }

        private double GetDistance()
        {
            var (dx, dy) = GetDeltas();
            double distance = Math.Sqrt( dx * dx + dy * dy );
            return distance;
        }
    }
}
