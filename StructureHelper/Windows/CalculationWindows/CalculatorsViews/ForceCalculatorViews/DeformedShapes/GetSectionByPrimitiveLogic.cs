using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Shapes.ConvertLogics;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class GetSectionByPrimitiveLogic : IGetSectionByPrimitiveLogic
    {
        public List<IDeformedSection> GetSection(INdmPrimitive primitive)
        {
            CheckObject.ThrowIfNull(primitive);
            List<ILinePolygonShape> linePolygons = GetPolygonsByPrimitiveShape(primitive);
            List<IDeformedSection> result = [];
            foreach (var linePolygon in linePolygons)
            {
                result.Add(GetSectionByLinePolygon(linePolygon));
            }
            return result;
        }

        private IDeformedSection GetSectionByLinePolygon(ILinePolygonShape polygonShape)
        {
            List<Vector2> vertices = [];
            foreach (var vertex in polygonShape.Vertices)
            {
                vertices.Add(new Vector2((float)vertex.Point.X, (float)vertex.Point.Y));
            }
            DeformedSection result = new DeformedSection() { Vertices = vertices};
            return result;

        }

        private List<ILinePolygonShape> GetPolygonsByPrimitiveShape(INdmPrimitive primitive)
        {
            List<ILinePolygonShape> result = [];
            if (primitive is IPointNdmPrimitive rebarPrimitive)
            {
                CircleShape circleShape = new() { Diameter = Math.Sqrt(rebarPrimitive.Area) * 0.785};
                var logic = new CircleShapeToPolygonConvertStrategy(true, 8);
                result.Add(logic.Convert(circleShape));
                return result;
            }
            var primitiveShape = primitive.Shape;
            if (primitiveShape is IRectangleShape rectangleShape)
            {
                var logic = new RectangleShapeToPolygonConvertStrategy();
                var polygon = logic.Convert(rectangleShape);
                result.Add(polygon);
            }
            if (primitiveShape is IEllipseShape ellipseShape && Math.Abs(ellipseShape.Width - ellipseShape.Height) < 1.0e-6)
            {
                CircleShape circleShape = new() { Diameter = ellipseShape.Width };
                var logic = new CircleShapeToPolygonConvertStrategy(true, 64);
                result.Add(logic.Convert(circleShape));
            }
            else if (primitiveShape is ILinePolygonShape linePolygonShape)
            {
                result.Add(linePolygonShape);
            }
            else if (primitiveShape is ITrapezoidShape trapezoidShape)
            {
                var logic = new TrapezoidShapeToPolygonConvertStrategy();
                var polygon = logic.Convert(trapezoidShape);
                result.Add(polygon);
            }
            else if (primitiveShape is IVerticalTShape tShape)
            {
                var logic = new VerticalTShapeToPolygonConvertStrategy();
                var polygon = logic.Convert(tShape);
                result.Add(polygon);
            }
            else if (primitiveShape is IVerticalDoubleTShape doubleTShape)
            {
                var logic = new VerticalDoubleTShapeToPolygonConvertStrategy();
                var polygon = logic.Convert(doubleTShape);
                result.Add(polygon);
            }
            else if (primitiveShape is IRingShape ringShape)
            {
                var logic = new RingShapeToPolygonsConvertStrategy()
                {
                    PartsCount = 64,
                    ArcSegmentsPerPart = 2
                };
                result.AddRange(logic.Convert(ringShape));
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(primitiveShape));
            }
            return result;
        }
    }
}
