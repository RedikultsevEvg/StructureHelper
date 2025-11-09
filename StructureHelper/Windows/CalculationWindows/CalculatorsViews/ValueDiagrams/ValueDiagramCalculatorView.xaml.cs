using System.Windows;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ValueDiagrams
{
    /// <summary>
    /// Логика взаимодействия для ValueDiagramCalculatorView.xaml
    /// </summary>
    public partial class ValueDiagramCalculatorView : Window
    {
        private ValueDiagramCalculatorViewModel vm;

        public ValueDiagramCalculatorView(ValueDiagramCalculatorViewModel vm)
        {
            this.vm = vm;
            vm.ParentWindow = this;
            this.DataContext = this.vm;
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.Refresh();
        }
    }
}
