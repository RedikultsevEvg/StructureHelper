using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace StructureHelper.Services.Exports
{
    /// <summary>
    /// Builds the table exported from the material diagram window.
    /// </summary>
    public static class MaterialDiagramTableBuilder
    {
        public static IReadOnlyList<string> GetHeaders(IEnumerable<LineSeries> series)
        {
            var headers = new List<string> { "Strain" };
            int index = 1;
            foreach (LineSeries line in series)
            {
                string title = string.IsNullOrWhiteSpace(line.Title)
                    ? $"Diagram {index}"
                    : line.Title;
                headers.Add(title);
                index++;
            }
            return headers;
        }

        public static IEnumerable<IReadOnlyList<object?>> GetRows(
            IReadOnlyList<string> labels,
            IEnumerable<LineSeries> series)
        {
            List<LineSeries> lines = series.ToList();
            List<List<double>> values = lines
                .Select(line => line.Values?.Cast<double>().ToList() ?? new List<double>())
                .ToList();

            int valueCount = values.Count == 0 ? 0 : values.Max(x => x.Count);
            int rowCount = valueCount > 0 ? valueCount : labels.Count;

            var rows = new List<IReadOnlyList<object?>>();
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var row = new List<object?>();
                row.Add(rowIndex < labels.Count ? ParseStrain(labels[rowIndex]) : null);
                foreach (List<double> column in values)
                {
                    row.Add(rowIndex < column.Count ? column[rowIndex] : null);
                }
                rows.Add(row);
            }
            return rows;
        }

        private static object? ParseStrain(string label)
        {
            if (double.TryParse(label, NumberStyles.Float, CultureInfo.CurrentCulture, out double value)
                || double.TryParse(label, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            return label;
        }
    }
}
