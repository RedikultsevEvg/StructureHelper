using ExcelDataReader;
using StructureHelper.Services.Exports;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace StructureHelper.Services.Points
{
    /// <summary>
    /// Exports a collection of points to a simple Excel workbook (*.xlsx) and imports it back.
    /// The workbook contains two columns, X and Y, in millimetres.
    /// </summary>
    public class PointsExcelService
    {
        public void Export(string fileName, IEnumerable<IPoint2D> points)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": file name");
            }
            if (points is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": points");
            }

            List<IReadOnlyList<object?>> rows = points
                .Select(point => (IReadOnlyList<object?>)new object?[]
                {
                    PointUnits.ToMillimeters(point.X),
                    PointUnits.ToMillimeters(point.Y)
                })
                .ToList();

            XlsxTableWriter.Write(fileName, "Points", new[] { "X, mm", "Y, mm" }, rows);
        }

        public IReadOnlyList<Point2D> Import(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || File.Exists(fileName) == false)
            {
                throw new StructureHelperException($"Excel file {fileName} does not exist");
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var points = new List<Point2D>();
            using FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
            while (reader.Read())
            {
                if (reader.FieldCount < 2)
                {
                    continue;
                }
                if (TryGetDouble(reader.GetValue(0), out double x) == false)
                {
                    continue;
                }
                if (TryGetDouble(reader.GetValue(1), out double y) == false)
                {
                    continue;
                }
                points.Add(new Point2D(PointUnits.FromMillimeters(x), PointUnits.FromMillimeters(y)));
            }
            return points;
        }

        private static bool TryGetDouble(object? value, out double result)
        {
            result = 0d;
            switch (value)
            {
                case double doubleValue:
                    result = doubleValue;
                    return true;
                case float floatValue:
                    result = floatValue;
                    return true;
                case int intValue:
                    result = intValue;
                    return true;
                case long longValue:
                    result = longValue;
                    return true;
                case decimal decimalValue:
                    result = (double)decimalValue;
                    return true;
                case string stringValue:
                    return double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out result)
                        || double.TryParse(stringValue, NumberStyles.Float, CultureInfo.CurrentCulture, out result);
                default:
                    return false;
            }
        }
    }
}
