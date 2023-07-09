﻿using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.ViewModels.Graphs
{
    public class GraphViewModel : ViewModelBase
    {
        public class ColumnInfo
        {
            public string Header { get; set; }
            public string BindingPath { get; set; }
        }


        IArrayParameter<double> arrayParameter;
        List<IValueParameter<double>> valueParameters;
        Dictionary<IValueParameter<double>, double[]> valueList;
        private RelayCommand redrawLinesCommand;
        private bool invertXValues;
        private bool invertYValues;

        public SelectedItemViewModel<IValueParameter<double>> XItems { get; private set; }
        public SelectItemsViewModel<IValueParameter<double>> YItems { get; set; }
        public ObservableCollection<ColumnInfo> Columns { get; } = new ObservableCollection<ColumnInfo>();


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

        private double lineSmoothness;

        public double LineSmoothness
        {
            get { return lineSmoothness; }
            set
            {
                value = Math.Max(value, 0d);
                value = Math.Min(value, 1d);
                value = Math.Round(value, 2);
                lineSmoothness = value;
                OnPropertyChanged(nameof(LineSmoothness));
            }
        }


        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public ICommand RedrawLinesCommand
        {
            get => redrawLinesCommand ??= new RelayCommand(o => DrawLines());
        }


        public GraphViewModel(IArrayParameter<double> arrayParameter)
        {
            this.arrayParameter = arrayParameter;
            valueParameters = GetParameters();
            XItems = new SelectedItemViewModel<IValueParameter<double>>(valueParameters);
            YItems = new SelectItemsViewModel<IValueParameter<double>>(valueParameters);
            YItems.ShowButtons = true;
            XItems.SelectedItem = XItems.Collection[0];
            YItems.UnSelectAllCommand.Execute(null);
            lineSmoothness = 0.3d;
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
        
        private void DrawLines()
        {
            if (XItems.SelectedItem is null || YItems.SelectedCount == 0) return;
            SetLines();
            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(Labels));
        }

        private void SetLines()
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
                    PointGeometry = null,
                    Stroke = new SolidColorBrush(yParameter.Color),
                    Fill = Brushes.Transparent,
                    LineSmoothness = lineSmoothness
                };
                _ = valueList.TryGetValue(xParameter, out double[] xValues);
                _ = valueList.TryGetValue(yParameter, out double[] yValues);
                var chartValues = new ChartValues<Point2D>();
                for (int i = 0; i < yValues.Count(); i++)
                {
                    double diagramValue = yValues[i] * yFactor;
                    var x = xValues[i] * xFactor;
                    var y = yValues[i] * yFactor;
                    var point = new Point2D() { X = x, Y =  y};
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