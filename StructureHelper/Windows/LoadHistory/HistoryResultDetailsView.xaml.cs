using System.Windows;

namespace StructureHelper.Windows.LoadHistory
{
    /// <summary>
    /// Window with additional information about one load history step.
    /// </summary>
    public partial class HistoryResultDetailsView : Window
    {
        public HistoryResultDetailsView(HistoryResultDetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
