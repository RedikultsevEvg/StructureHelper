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
        public static void SetVisualProps(LineSeries lineSeries, GraphVisualProps visualProps, Color lineColor)
        {
            lineSeries.Stroke = new SolidColorBrush(lineColor);
            SetVisualProps(lineSeries, visualProps);
            lineSeries.Fill = new SolidColorBrush(lineColor) { Opacity = visualProps.Opacity };
        }
        public static void SetVisualProps(LineSeries lineSeries, GraphVisualProps visualProps)
        {
            lineSeries.LineSmoothness = visualProps.LineSmoothness;
            lineSeries.PointGeometry = DefaultGeometries.Circle;
            lineSeries.PointGeometrySize = visualProps.StrokeSize;
            Color lineColor = (lineSeries.Stroke as SolidColorBrush)?.Color ?? Colors.Black;
            //lineSeries.Fill = new SolidColorBrush(lineColor) { Opacity = visualProps.Opacity };
            lineSeries.Fill = new SolidColorBrush(lineColor) { Opacity = visualProps.Opacity };
            
        }
    }
}
