using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.InfraStructures.Exceptions;
using FieldVisualizer.InfraStructures.Strings;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Windows.UserControls;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public class AddPrimitivesToCanvasLogic
    {
        private const int zoomFactor = 1000;
        private IMathRoundLogic roundLogic = new SmartRoundLogic() { DigitQuant = 3 };
        public Canvas WorkPlaneCanvas { get; set; }
        public IValueRange ValueRange { get; set; }
        public IColorMap ColorMap { get; set; }
        public IEnumerable<IValueColorRange> ValueColorRanges { get; set; }
        public double DX { get; set; }
        public double DY { get; set; }
        public double ValueLabelZoomFactor { get; set; } = 1;
        public double YZoomFactor { get; set; } = 1;

        public void ProcessPrimitives(IEnumerable<IValuePrimitive> valuePrimitives)
        {
            foreach (var primitive in valuePrimitives)
            {
                if (primitive is IRectanglePrimitive rectanglePrimitive)
                {
                    Rectangle rectangle = ProcessRectanglePrimitive(rectanglePrimitive);
                    WorkPlaneCanvas.Children.Add(rectangle);
                }
                else if (primitive is ICirclePrimitive circlePrimitive)
                {
                    Ellipse ellipse = ProcessCirclePrimitive(circlePrimitive);
                    WorkPlaneCanvas.Children.Add(ellipse);
                }
                else if (primitive is ITrianglePrimitive triangle)
                {
                    Path path = ProcessTrianglePrimitive(triangle);
                    WorkPlaneCanvas.Children.Add(path);
                }
                else { throw new FieldVisulizerException(ErrorStrings.PrimitiveTypeIsUnknown); }
            }
        }

        private Path ProcessTrianglePrimitive(ITrianglePrimitive triangle)
        {
            // Create the PathFigure using triangle vertices.
            var figure = new PathFigure
            {
                StartPoint = new Point(triangle.Point1.X * zoomFactor, YZoomFactor * triangle.Point1.Y * zoomFactor),
                IsClosed = true,
                IsFilled = true
            };

            // Add the remaining vertices as LineSegments
            var segments = new PathSegmentCollection
        {
            new LineSegment(new Point(triangle.Point2.X * zoomFactor,YZoomFactor * triangle.Point2.Y * zoomFactor), true),
            new LineSegment(new Point(triangle.Point3.X * zoomFactor, YZoomFactor * triangle.Point3.Y * zoomFactor), true)
            // Closing is handled by IsClosed = true, so we don't need to add a segment back to Point1
        };
            figure.Segments = segments;

            // Create geometry and path
            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            var path = new Path
            {
                Data = geometry,
            };
            ProcessShape(path, triangle, 0, 0, false);
            path.MouseLeftButtonDown += OnPathClicked;
            return path;
        }
        private Rectangle ProcessRectanglePrimitive(IRectanglePrimitive rectanglePrimitive)
        {
            Rectangle rectangle = new Rectangle
            {
                Height = rectanglePrimitive.Height * zoomFactor,
                Width = rectanglePrimitive.Width * zoomFactor
            };
            double addX = rectanglePrimitive.Width / 2 * zoomFactor;
            double addY = rectanglePrimitive.Height / 2 * zoomFactor;
            ProcessShape(rectangle, rectanglePrimitive, addX, addY, true);
            rectangle.MouseLeftButtonDown += OnRectangleClicked;
            return rectangle;
        }
        private Ellipse ProcessCirclePrimitive(ICirclePrimitive circlePrimitive)
        {
            Ellipse ellipse = new Ellipse
            {
                Height = circlePrimitive.Diameter * zoomFactor,
                Width = circlePrimitive.Diameter * zoomFactor
            };
            double addX = circlePrimitive.Diameter / 2 * zoomFactor;
            double addY = circlePrimitive.Diameter / 2 * zoomFactor;

            ProcessShape(ellipse, circlePrimitive, addX, addY, true);
            ellipse.MouseLeftButtonDown += OnEllipseClicked;
            return ellipse;
        }
        private void ProcessShape(Shape shape, IValuePrimitive valuePrimitive, double addX, double addY, bool addCenter)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = ColorOperations.GetColorByValue(ValueRange, ColorMap, valuePrimitive.Value);
            foreach (var valueRange in ValueColorRanges)
            {
                if (valuePrimitive.Value >= valueRange.ExactValues.BottomValue & valuePrimitive.Value <= valueRange.ExactValues.TopValue & (!valueRange.IsActive))
                {
                    brush.Color = Colors.Gray;
                }
            }
            shape.ToolTip = roundLogic.RoundValue(valuePrimitive.Value);
            shape.Tag = valuePrimitive;
            shape.Fill = brush;
            shape.StrokeThickness = 0.0;
            double addLeft = -addX - DX * zoomFactor;
            double addTop = -addY - YZoomFactor * DY * zoomFactor;
            if (addCenter == true)
            {
                addLeft += valuePrimitive.CenterX * zoomFactor;
                addTop += YZoomFactor * valuePrimitive.CenterY * zoomFactor;
            }
            Canvas.SetLeft(shape, addLeft);
            Canvas.SetTop(shape, addTop);
        }

        private void OnRectangleClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Rectangle rectangle)
                return;

            if (rectangle.Tag is not IRectanglePrimitive primitive)
                return;
            AddLabel(primitive);
            e.Handled = true;
        }

        private void OnEllipseClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Ellipse ellipse)
                return;

            if (ellipse.Tag is not ICirclePrimitive primitive)
                return;
            AddLabel(primitive);
            e.Handled = true;
        }

        private void AddLabel(IValuePrimitive primitive)
        {
            var value = roundLogic.RoundValue(primitive.Value);

            var label = new ValueLabelControl()
            {
                Value = Convert.ToString(value),
                ScaleX = WorkPlaneCanvas.Height * ValueLabelZoomFactor,
                ScaleY = WorkPlaneCanvas.Height * ValueLabelZoomFactor * YZoomFactor * (-1),
                X = (primitive.CenterX - DX) * zoomFactor,
                Y = (YZoomFactor * primitive.CenterY + DY) * zoomFactor,
            };

            WorkPlaneCanvas.Children.Add(label);
        }

        private void OnPathClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Path path)
                return;

            if (path.Tag is not ITrianglePrimitive primitive)
                return;        

            AddLabel(primitive);
            e.Handled = true;
        }
    }
}
