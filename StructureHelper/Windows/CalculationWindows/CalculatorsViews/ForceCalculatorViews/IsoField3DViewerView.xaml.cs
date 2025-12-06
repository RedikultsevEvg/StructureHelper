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
    /// Interaction logic for ISoField3DViewerView.xaml
    /// </summary>
    public partial class IsoField3DViewerView : Window
    {
        IsoField3DViewerViewModel viewModel;

        public IsoField3DViewerView(IsoField3DViewerViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            viewModel.ViewportViewModel.Viewport3D = View3D;
            viewModel.SaveCopyViewModel.FrameWorkElement = ViewportGrid;
        }
    }
}
