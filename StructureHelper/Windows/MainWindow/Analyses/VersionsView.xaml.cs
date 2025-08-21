using StructureHelperCommon.Models.Analyses;
using System.Windows;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    /// <summary>
    /// Interaction logic for VersionsView.xaml
    /// </summary>
    public partial class VersionsView : Window
    {
        VersionsViewModel viewModel;
        public VersionsView(VersionsViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            if (this.viewModel.OwnerWindow is not null)
            {
                Owner = this.viewModel.OwnerWindow;
            }
        }

        public VersionsView(IVersionProcessor versionProcessor) : this (new VersionsViewModel(versionProcessor))
        {
            
        }
    }
}
