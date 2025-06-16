using System.Windows;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    /// <summary>
    /// Interaction logic for SelectResultsForExportView.xaml
    /// </summary>
    public partial class SelectResultsForExportView : Window
    {
        private SelectResultsForExportViewModel viewModel;
        public SelectResultsForExportView(SelectResultsForExportViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
        }
    }
}
