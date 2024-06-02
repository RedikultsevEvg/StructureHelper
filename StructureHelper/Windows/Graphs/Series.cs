using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.Graphs
{
    public class Series : ViewModelBase
    {
        private IArrayParameter<double> arrayParameter;
        private List<IValueParameter<double>> valueParameters;
        private Dictionary<IValueParameter<double>, double[]> valueList;
        private bool invertXValues;
        private bool invertYValues;

        public SelectItemVM<IValueParameter<double>> XItems { get; }
        public SelectItemsVM<IValueParameter<double>> YItems { get; }
        public bool InvertXValues
        {
            get { return invertXValues; }
            set
            {
                invertXValues = value;
                OnPropertyChanged(nameof(InvertXValues));
            }
        }
        public bool InvertYValues
        {
            get { return invertYValues; }
            set
            {
                invertYValues = value;
                OnPropertyChanged(nameof(InvertYValues));
            }
        }

        public GraphVisualProps VisualProps { get; set; }
        public SeriesCollection SeriesCollection { get; private set; }
        public List<string> Labels { get; private set; }
        public Color Color { get; private set; }

        public string Name { get; set; }
        public IArrayParameter<double> ArrayParameter { get; set; }
        public Series(IArrayParameter<double> arrayParameter)
        {
            this.arrayParameter = arrayParameter;
            valueParameters = GetParameters();
            XItems = new SelectItemVM<IValueParameter<double>>(valueParameters);
            YItems = new SelectItemsVM<IValueParameter<double>>(valueParameters);
            YItems.ShowButtons = true;
            XItems.SelectedItem = XItems.Collection[0];
            YItems.UnSelectAllCommand.Execute(null);
            VisualProps = new();
            Color = ColorProcessor.GetRandomColor();
        }

        private List<IValueParameter<double>> GetParameters()
        {
            valueList = new Dictionary<IValueParameter<double>, double[]>();
            var items = new List<IValueParameter<double>>();
            var data = arrayParameter.Data;
            int columnCount = data.GetLength(1);
            for (int i = 0; i < columnCount; i++)
            {
                var item = new ValueParameter<double>()
                {
                    Name = arrayParameter.ColumnLabels[i],
                    Color = ColorProcessor.GetRandomColor(),
                };
                items.Add(item);
                int rowCount = data.GetLength(0);
                var values = new double[rowCount];
                for (int j = 0; j < rowCount; j++)
                {
                    values[j] = data[j, i];
                }
                valueList.Add(item, values);
            }
            return items;
        }

        public void DrawLines()
        {
            if (XItems.SelectedItem is null || YItems.SelectedCount == 0) return;
            RefreshSeries();
            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(Labels));
        }

        public void RefreshSeries()
        {
            var xParameter = XItems.SelectedItem;
            var yParameters = YItems.SelectedItems;
            var xFactor = invertXValues ? -1d : 1d;
            var yFactor = invertYValues ? -1d : 1d;
            var labels = new List<double>();
            SeriesCollection = new SeriesCollection();
            foreach (var yParameter in yParameters)
            {
                var localLabels = new List<double>();
                var lineSeries = new LineSeries()
                {
                    Configuration = new CartesianMapper<IPoint2D>()
                    .X(point => point.X)
                    .Y(point => point.Y),
                    Title = yParameter.Name,
                };
                GraphService.SetVisualProps(lineSeries, VisualProps, yParameter.Color);
                _ = valueList.TryGetValue(xParameter, out double[] xValues);
                _ = valueList.TryGetValue(yParameter, out double[] yValues);
                var chartValues = new ChartValues<Point2D>();
                for (int i = 0; i < yValues.Count(); i++)
                {

                    double diagramValue = yValues[i] * yFactor;
                    var x = xValues[i] * xFactor;
                    var y = yValues[i] * yFactor;
                    var point = new Point2D() { X = x, Y = y };
                    chartValues.Add(point);
                    labels.Add(x);
                    localLabels.Add(x);

                }
                lineSeries.Values = chartValues;
                //lineSeries.LabelPoint = point => localLabels[(int)point.X].ToString();
                SeriesCollection.Add(lineSeries);
            }
            Labels = labels
                .OrderBy(x => x)
                .Distinct()
                .Select(x => x.ToString())
                .ToList();
        }
    }
}
