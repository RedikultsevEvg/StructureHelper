using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class GetSectionByPrimitiveLogic : IGetSectionByPrimitiveLogic
    {
        public List<Vector2> GetSection(INdmPrimitive primitive)
        {
            CheckObject.ThrowIfNull(primitive);
            ILinePolygonShape linePolygon = GetPolygonByPrimitiveShape(primitive);
            List<Vector2> result = GetSectionByLinePolygon(linePolygon);
            return result;
        }

        private List<Vector2> GetSectionByLinePolygon(ILinePolygonShape polygonShape)
        {
            List<Vector2> section = [];
            foreach (var vertex in polygonShape.Vertices)
            {
                section.Add(new Vector2((float)vertex.Point.X, (float)vertex.Point.Y));
            }
            return section;
        }

        private ILinePolygonShape GetPolygonByPrimitiveShape(INdmPrimitive primitive)
        {
            if (primitive is IRebarNdmPrimitive rebarPrimitive)
            {
                CircleShape circleShape = new() { Diameter = Math.Sqrt(rebarPrimitive.Area) * 0.785};
                var logic = new CircleShapeToPolygonConvertStrategy(true, 8);
                return logic.Convert(circleShape);
            }
            if (primitive.Shape is IRectangleShape rectangleShape)
            {
                var logic = new RectangleShapeToPolygonConvertStrategy();
                var polygon = logic.Convert(rectangleShape);
                return polygon;
            }
            if (primitive.Shape is IEllipseShape ellipseShape && Math.Abs(ellipseShape.Width - ellipseShape.Height) < 1.0e-6)
            {
                CircleShape circleShape = new() { Diameter = ellipseShape.Width };
                var logic = new CircleShapeToPolygonConvertStrategy(true, 64);
                return logic.Convert(circleShape);
            }
            else if (primitive.Shape is ILinePolygonShape linePolygonShape)
            {
                return linePolygonShape;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(primitive.Shape));
            }
        }
    }
}
