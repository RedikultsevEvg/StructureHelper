using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StructureHelper.Services.Points
{
    /// <summary>
    /// Converts a collection of points to tab-separated text and back. The text can be pasted into
    /// Excel and copied from Excel. Coordinates are expressed in millimetres.
    /// </summary>
    public static class PointTextSerializer
    {
        public const string Header = "X, mm\tY, mm";

        public static string Serialize(IEnumerable<IPoint2D> points, bool includeHeader = true)
        {
            var builder = new StringBuilder();
            if (includeHeader)
            {
                builder.AppendLine(Header);
            }
            foreach (IPoint2D point in points)
            {
                builder
                    .Append(Format(PointUnits.ToMillimeters(point.X)))
                    .Append('\t')
                    .Append(Format(PointUnits.ToMillimeters(point.Y)))
                    .AppendLine();
            }
            return builder.ToString();
        }

        public static List<Point2D> Deserialize(string? text)
        {
            var points = new List<Point2D>();
            if (string.IsNullOrWhiteSpace(text))
            {
                return points;
            }

            foreach (string rawLine in text.Split('\n'))
            {
                string line = rawLine.Trim('\r', ' ', '\t');
                if (line.Length == 0)
                {
                    continue;
                }

                string[] cells = line.Split('\t');
                if (cells.Length < 2)
                {
                    cells = line.Split(';');
                }
                if (cells.Length < 2)
                {
                    continue;
                }

                if (TryParse(cells[0], out double x) && TryParse(cells[1], out double y))
                {
                    points.Add(new Point2D(PointUnits.FromMillimeters(x), PointUnits.FromMillimeters(y)));
                }
            }
            return points;
        }

        private static string Format(double value) => value.ToString("G17", CultureInfo.InvariantCulture);

        private static bool TryParse(string value, out double result)
        {
            string text = value.Trim();
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out result)
                || double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out result);
        }
    }
}
