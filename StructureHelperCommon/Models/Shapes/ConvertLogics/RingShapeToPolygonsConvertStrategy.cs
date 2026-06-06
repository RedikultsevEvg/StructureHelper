using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes.ConvertLogics
{
    public class RingShapeToPolygonsConvertStrategy
        : IObjectConvertStrategy<IReadOnlyList<ILinePolygonShape>, IRingShape>
    {
        public int PartsCount { get; set; } = 2;

        /// <summary>
        /// Number of arc segments used for each part.
        /// </summary>
        public int ArcSegmentsPerPart { get; set; } = 16;

        public IReadOnlyList<ILinePolygonShape> Convert(IRingShape source)
        {
            if (source == null)
                throw new StructureHelperException("Ring shape is null.");

            if (PartsCount < 2)
                throw new StructureHelperException(
                    "PartsCount must be at least 2.");

            if (source.InnerDiameter >= source.OuterDiameter)
                throw new StructureHelperException(
                    "Inner diameter must be smaller than outer diameter.");

            var result = new List<ILinePolygonShape>();

            double outerRadius = source.OuterDiameter / 2.0;
            double innerRadius = source.InnerDiameter / 2.0;

            double sectorAngle = 2.0 * Math.PI / PartsCount;

            for (int part = 0; part < PartsCount; part++)
            {
                double startAngle = part * sectorAngle;
                double endAngle = startAngle + sectorAngle;

                var polygon = new LinePolygonShape();

                // Outer arc
                for (int i = 0; i <= ArcSegmentsPerPart; i++)
                {
                    double t = (double)i / ArcSegmentsPerPart;
                    double angle = startAngle + t * (endAngle - startAngle);

                    polygon.AddVertex(
                        new Vertex(
                            outerRadius * Math.Cos(angle),
                            outerRadius * Math.Sin(angle)));
                }

                // Inner arc (reverse direction)
                for (int i = ArcSegmentsPerPart; i >= 0; i--)
                {
                    double t = (double)i / ArcSegmentsPerPart;
                    double angle = startAngle + t * (endAngle - startAngle);

                    polygon.AddVertex(
                        new Vertex(
                            innerRadius * Math.Cos(angle),
                            innerRadius * Math.Sin(angle)));
                }

                polygon.IsClosed = true;

                result.Add(polygon);
            }

            return result;
        }
    }
}
