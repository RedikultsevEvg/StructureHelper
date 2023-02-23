using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Infrastructure.Commands;
using FieldVisualizer.InfraStructures.Enums;
using FieldVisualizer.InfraStructures.Exceptions;
using FieldVisualizer.InfraStructures.Strings;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.Services.ValueRanges;
using FieldVisualizer.ViewModels.FieldViewerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FieldVisualizer.Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FieldViewer.xaml
    /// </summary>
    public partial class FieldViewer : UserControl
    {
        private FieldViewerViewModel viewModel;

        public FieldViewer()
        {
            InitializeComponent();
            viewModel = new FieldViewerViewModel();
            this.DataContext = viewModel;
            PrimitiveSet = viewModel.PrimitiveSet;
            viewModel.WorkPlaneBox = WorkPlaneBox;
            viewModel.WorkPlaneCanvas = WorkPlaneCanvas;
            viewModel.Legend = LegendViewer;
        }

        //public FieldViewer(FieldViewerViewModel vm)
        //{
        //    InitializeComponent();
        //    viewModel = vm;
        //    this.DataContext = viewModel;
        //    PrimitiveSet = viewModel.PrimitiveSet;
        //    viewModel.WorkPlaneBox = WorkPlaneBox;
        //    viewModel.WorkPlaneCanvas = WorkPlaneCanvas;
        //    viewModel.Legend = LegendViewer;
        //}

        public IPrimitiveSet PrimitiveSet { get => viewModel.PrimitiveSet; set { viewModel.PrimitiveSet = value; } }

        internal void Refresh()
        {
            viewModel.Refresh();
        }

        private void WorkPlaneViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollViewer viewer = (ScrollViewer)sender;
            viewModel.ScrolWidth = viewer.ActualWidth;
            viewModel.ScrolHeight = viewer.ActualHeight;
        }
    }
}
