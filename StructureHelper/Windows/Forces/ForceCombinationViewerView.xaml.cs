using StructureHelperCommon.Models.Forces;
using System.Windows;

namespace StructureHelper.Windows.Forces
{
    /// <summary>
    /// Interaction logic for ForceCombinationViewerView.xaml
    /// </summary>
    public partial class ForceCombinationViewerView : Window
    {
        ForceCombinationViewerVM viewModel;
        public ForceCombinationViewerView(ForceCombinationViewerVM viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
        }

        public ForceCombinationViewerView(IForceAction action) : this (new ForceCombinationViewerVM(action))
        {
        }
    }
}
