using LiveCharts;
using LiveCharts.Wpf;
using StructureHelperCommon.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StructureHelperCommon.Services
{
    public class GraphSettings
    {
        private Color _strokeColor;
        private Color _fillColor;
        private List<string> _labels = new List<string>();
        private SeriesCollection _seriesCollection = new SeriesCollection();
        private LineSeries _lineSeries { get; set; } = new LineSeries();
        private ChartValues<double> _chartValues { get; set; } = new ChartValues<double>();
        public List<GraphPoint> GraphPoints { get; set; } = new List<GraphPoint>();
        public GraphSettings(Color strokeColor, Color fillColor)
        {
            _strokeColor = strokeColor;
            _fillColor = fillColor;
        }
        public List<string> GetLabels()
        {
            foreach (GraphPoint point in GraphPoints)
            {
                _labels.Add(Math.Round(point.X, 2).ToString());
            }
            return _labels;
        }
        public SeriesCollection GetSeriesCollection()
        {
            foreach (GraphPoint point in GraphPoints)
            {
                _chartValues.Add(Math.Round(point.Y, 2));
            }
            _lineSeries.Values = _chartValues;
            _lineSeries.Stroke = new SolidColorBrush(_strokeColor);
            _lineSeries.Fill = Brushes.Transparent;
            _seriesCollection.Add(_lineSeries);
            return _seriesCollection;
        }
    }
}
