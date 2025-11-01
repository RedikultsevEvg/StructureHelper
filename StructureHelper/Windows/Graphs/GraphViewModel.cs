using LiveCharts;
using LiveCharts.Wpf;
using StructureHelper.Infrastructure;
using StructureHelper.Services.Exports;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Exports;
using StructureHelperCommon.Services.Exports.Factories;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml.Linq;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.Graphs
{
    public class GraphViewModel : ViewModelBase
    {
        private IFrameWorkElementServiseLogic frameWorkElementServiseLogic = new FrameWorkElementServiseLogic();
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
        private RelayCommand copyToClipboardCommand;

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
            var inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Csv);
            var logic = new ExportChartToCSVLogic(Series);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }

        public ICommand SaveAsImage
        {
            get => saveImageCommand ??= new RelayCommand(o => frameWorkElementServiseLogic.SaveImageToFile(MainChart));
        }

        public ICommand CopyToClipboardCommand
        {
            get => copyToClipboardCommand ??= new RelayCommand(o => frameWorkElementServiseLogic.CopyImageToClipboard(MainChart));
        }


        public CartesianChart MainChart { get; set; }

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
