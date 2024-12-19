using LiveCharts;
using LiveCharts.Wpf;
using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.Graphs
{
    public class GraphViewModel : ViewModelBase
    {
        public class ColumnInfo
        {
            public string Header { get; set; }
            public string BindingPath { get; set; }
        }


        List<IArrayParameter<double>> arrayParameters;

        private RelayCommand redrawLinesCommand;
        private bool invertXValues;
        private bool invertYValues;
        private RelayCommand saveImageCommand;
        private RelayCommand exportToCSVCommand;

        public SelectItemVM<IValueParameter<double>> XItems { get; private set; }
        public SelectItemsVM<IValueParameter<double>> YItems { get; set; }
        public ObservableCollection<ColumnInfo> Columns { get; } = new ObservableCollection<ColumnInfo>();
        public ObservableCollection<Series> Series { get;}


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

        public GraphVisualProps VisualProps { get; }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public ICommand RedrawLinesCommand
        {
            get => redrawLinesCommand ??= new RelayCommand(o => DrawSeries());
        }

        public ICommand ExportToCSVCommand
        {
            get => exportToCSVCommand ??= new RelayCommand(o => ExportSeries());
        }

        private void ExportSeries()
        {
            throw new NotImplementedException();
        }

        public ICommand SaveAsImage
        {
            get => saveImageCommand ??= new RelayCommand(o => SaveImage());
        }
        public CartesianChart MainChart { get; set; }

        public void ExportChartToImage(CartesianChart chart, string filePath)
        {
            // Measure and arrange the chart to ensure it's fully rendered
            chart.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            chart.Arrange(new System.Windows.Rect(0, 0, chart.ActualWidth, chart.ActualHeight));

            // Create a RenderTargetBitmap
            var renderTarget = new RenderTargetBitmap(
                (int)chart.ActualWidth,
                (int)chart.ActualHeight,
                96, // DPI X
                96, // DPI Y
                PixelFormats.Pbgra32);

            // Render the chart to the bitmap
            renderTarget.Render(chart);

            // Save as a PNG
            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                pngEncoder.Save(stream);
            }
        }

        private void SaveImage()
        {
            var inputData = new ExportToFileInputData();
            inputData.FileName = "New File";
            inputData.Filter = "png |*.png";
            inputData.Title = "Save in png File";

            //var viewbox = new Viewbox();
            //viewbox.Child = MainChart;
            //viewbox.Measure(MainChart.RenderSize);
            //viewbox.Arrange(new Rect(new Point(0, 0), MainChart.RenderSize));
            //MainChart.Update(true, true); //force chart redraw
            //viewbox.UpdateLayout();

            var logic = new ExportFrameWorkElementLogic(MainChart);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }


        private void CopyImageToClipboard(BitmapImage bitmapImage)
        {
            Clipboard.SetImage(bitmapImage);
        }

        public GraphViewModel(IArrayParameter<double> arrayParameter) : this (new List<IArrayParameter<double>>() { arrayParameter})
        {

        }

        public GraphViewModel(IEnumerable<IArrayParameter<double>> arrayParameters)
        {
            this.arrayParameters = arrayParameters.ToList();
            Series = new();
            foreach (var item in this.arrayParameters)
            {
                Series.Add(new Series(item));
            }
            VisualProps = new();
        }

        public GraphViewModel(IEnumerable<Series> series)
        {
            Series = new();
            foreach (var item in series)
            {
                Series.Add(item);
            }
            VisualProps = new();
        }



        private void DrawSeries()
        {
            SetLines();
            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(Labels));
        }

        private void SetLines()
        {
            SeriesCollection = new SeriesCollection();
            Labels = new();
            foreach (var series in Series)
            {
                series.VisualProps = VisualProps;
                series.RefreshSeries();
                SeriesCollection.AddRange(series.SeriesCollection);
                Labels.AddRange(series.Labels);
            }
        }
    }
}
