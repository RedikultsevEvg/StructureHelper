using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;

namespace StructureHelper.Windows.Graphs
{
    internal static class GraphService
    {
        public static void SetVisualProps(LineSeries lineSeries, GraphVisualProps visualProps, Color color)
        {
            lineSeries.Stroke = new SolidColorBrush(color);
            lineSeries.Fill = new SolidColorBrush(color) { Opacity = visualProps.Opacity };
            lineSeries.LineSmoothness = visualProps.LineSmoothness;
            lineSeries.PointGeometry = DefaultGeometries.Circle;
            lineSeries.PointGeometrySize = visualProps.StrokeSize;
        }
    }
}
