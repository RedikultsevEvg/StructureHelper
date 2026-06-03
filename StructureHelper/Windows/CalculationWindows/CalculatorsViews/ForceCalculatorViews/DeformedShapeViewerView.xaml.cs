using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    /// <summary>
    /// Interaction logic for DeformedShapeViewerView.xaml
    /// </summary>
    public partial class DeformedShapeViewerView : Window
    {
        private DeformedShapeViewerViewModel viewModel;
        public DeformedShapeViewerView(DeformedShapeViewerViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = viewModel;
            viewModel.ViewportViewModel.Viewport3D = View3D;
            viewModel.SaveCopyViewModel.FrameWorkElement = ViewportGrid;
            viewModel.Rebuild();
        }
    }
}
