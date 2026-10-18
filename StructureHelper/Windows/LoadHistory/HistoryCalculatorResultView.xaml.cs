using StructureHelperLogics.Models.LoadHistory;
using System.Windows;

namespace StructureHelper.Windows.LoadHistory
{
    /// <summary>
    /// Interaction logic for HistoryCalculatorResultView.xaml
    /// </summary>
    public partial class HistoryCalculatorResultView : Window
    {
        HistoryCalculatorResultViewModel viewModel;
        public HistoryCalculatorResultView(HistoryCalculatorResultViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }

        public HistoryCalculatorResultView(IHistoryCalculatorResult result) : this(new HistoryCalculatorResultViewModel(result))
        {
        }
    }
}
