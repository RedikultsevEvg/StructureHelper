using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.ViewModels.FieldViewerViewModels;
using System.Windows;
using System.Windows.Controls;

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
