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
    /// Interaction logic for IsoField2DViewerView.xaml
    /// </summary>
    public partial class IsoField2DViewerView : Window
    {
        private IsoField2DViewerViewModel viewModel;

        public IsoField2DViewerView(IsoField2DViewerViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            this.viewModel.Window = this;
        }
    }
}
