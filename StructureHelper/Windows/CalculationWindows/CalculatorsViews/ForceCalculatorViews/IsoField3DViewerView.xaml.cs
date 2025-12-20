using System.Windows;

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
