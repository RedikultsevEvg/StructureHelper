using System.Windows;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ValueDiagrams
{
    /// <summary>
    /// Логика взаимодействия для ValueDiagramEntityView.xaml
    /// </summary>
    public partial class ValueDiagramEntityView : Window
    {
        private readonly ValueDiagramEntityViewModel viewModel;

        public ValueDiagramEntityView(ValueDiagramEntityViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
            viewModel.ParentWindow = this;
        }
    }
}
