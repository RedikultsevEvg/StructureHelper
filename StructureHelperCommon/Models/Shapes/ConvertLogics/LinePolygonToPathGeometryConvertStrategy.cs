using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System.Linq;
using System.Windows.Media;

namespace StructureHelperCommon.Models.Shapes
{
    public class LinePolygonToPathGeometryConvertStrategy : IObjectConvertStrategy<PathGeometry, ILinePolygonShape>
    {
        public double ScaleX { get; set; } = 1.0;
        public double ScaleY { get; set; } = 1.0;
        public double CenterX { get; set; } = 0.0;
        public double CenterY { get; set; } = 0.0;
        public PathGeometry Convert(ILinePolygonShape source)
        {
            ILinePolygonShape scaledPolygon = GetScaledPolygon(source);
            if (scaledPolygon.Vertices.Count == 0)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": polygon does not contain any vertices");
            }
            var points = scaledPolygon.Vertices.Select(x => x.Point).ToList();
            IPoint2D StartPoint = points[0];
            System.Windows.Point systemPoint = GetSystemPoint(StartPoint);
            var figure = new PathFigure { StartPoint = systemPoint };
            for (int i = 1; i < points.Count; i++)
                figure.Segments.Add(new LineSegment(GetSystemPoint(points[i]), true));
            figure.IsClosed = true;
            PathGeometry pathGeometry = new PathGeometry([figure]);
            return pathGeometry;
        }

        private ILinePolygonShape GetScaledPolygon(ILinePolygonShape source)
        {
            var scaleStrategy = new LinePolygonScaleStrategy()
            {
                CenterX = CenterX,
                CenterY = CenterY,
                ScaleX = ScaleX,
                ScaleY = ScaleY,
            };
            var scaledPolygon = scaleStrategy.Convert(source);
            return scaledPolygon;
        }

        private System.Windows.Point GetSystemPoint(IPoint2D helperPoint)
        {
            return new(helperPoint.X, helperPoint.Y);
        }
    }
}
