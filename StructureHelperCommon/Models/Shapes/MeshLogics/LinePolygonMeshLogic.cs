using StructureHelperCommon.Infrastructures.Exceptions;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Meshing;

namespace StructureHelperCommon.Models.Shapes
{
    /// <summary>
    /// Logic of mesh of line polygon with Triangle lib
    /// </summary>
    public class LinePolygonMeshLogic : IMeshShapeLogic
    {
        private ILinePolygonShape linePolygonShape;
        /// <inheritdoc/>
        public ICenterShape CenterShape { get; set; }
        /// <inheritdoc/>
        public double MaximumMeshSize { get; set; } = 0.01;
        /// <inheritdoc/>
        public double MinimumAngleInDegree { get; set; } = 25.0;

        /// <inheritdoc/>
        public IMesh Triangulate()
        {
            if (CenterShape.Shape is not ILinePolygonShape polygonShape)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(CenterShape.Shape) + ": Shape is not line polygon shape");
            }
            linePolygonShape = polygonShape;
            Polygon polygon = GetTrianglePolygon();
            var quality = new QualityOptions()
            {
                MinimumAngle = 25.0,
                MaximumArea = MaximumMeshSize * MaximumMeshSize,
            };
            var mesh = polygon.Triangulate(quality);
            return mesh;
        }

        private Polygon GetTrianglePolygon()
        {
            var logic = new LinePolygonTranslateStrategy()
            {
                DeltaX = CenterShape.Center.X,
                DeltaY = CenterShape.Center.Y,
            };
            var polygon = new Polygon();
            ILinePolygonShape centeredPolygon = logic.Convert(linePolygonShape);
            List<IVertex> vertices = centeredPolygon.Vertices.ToList();
            var contourConvertLogic = new LinePolygonToMeshContourConvertStrategy();
            var contour = contourConvertLogic.Convert(centeredPolygon);
            // Add contour to polygon — this automatically defines the connecting segments
            polygon.Add(contour);
            return polygon;
        }
    }
}
