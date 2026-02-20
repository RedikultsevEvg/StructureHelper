using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using TriangleNet.Geometry;
using TriangleNet.Meshing;

namespace StructureHelperCommon.Models.Shapes
{
    internal class RingShapeMeshLogic : IMeshShapeLogic
    {
        private const int minSegmentNumber = 16;
        private const int minRingThicknesDivision = 2;
        private IRingShape ringShape;

        public ICenterShape CenterShape { get; set; }
        public double MinimumAngleInDegree { get; set; } = 25.0;
        public double MaximumMeshSize { get; set; } = 0.01;

        public IMesh Triangulate()
        {
            if (CenterShape.Shape is not IRingShape ringShape)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(CenterShape.Shape) + ": Shape is not a ring shape");
            }
            this.ringShape = ringShape;
            Polygon polygon = new Polygon();
            polygon.Add(GetContour(this.ringShape.OuterDiameter, true));
            polygon.Add(GetContour(this.ringShape.InnerDiameter, false));
            polygon.Holes.Add(new Point(CenterShape.Center.X, CenterShape.Center.Y));
            var options = new ConstraintOptions
            {
                ConformingDelaunay = true
            };
            double maximumSize = Math.Min(MaximumMeshSize, (ringShape.OuterRadius - ringShape.InnerRadius) / minRingThicknesDivision);
            var quality = new QualityOptions()
            {
                MinimumAngle = 25.0,
                MaximumArea = maximumSize * maximumSize,
            };
            var mesh = polygon.Triangulate(options, quality);
            return mesh;
        }

        private Contour GetContour(double diameter, bool isCounterClockwise)
        {
            CircleShape circle = new() { Diameter = diameter };
            int segments = (int)Convert.ToInt64(Math.Ceiling(Math.PI * diameter / MaximumMeshSize));
            segments = Math.Max(segments, minSegmentNumber);
            CircleShapeToPolygonConvertStrategy convertStrategy = new(isCounterClockwise, segments);
            var linePolygon = convertStrategy.Convert(circle);
            LinePolygonTranslateStrategy transformStrategy = new()
            {
                DeltaX = CenterShape.Center.X,
                DeltaY = CenterShape.Center.Y,
            };
            var centeredPolygon = transformStrategy.Convert(linePolygon);
            LinePolygonToMeshContourConvertStrategy contourStrategy = new();
            var contour = contourStrategy.Convert(centeredPolygon);
            return contour;
        }
    }
}
