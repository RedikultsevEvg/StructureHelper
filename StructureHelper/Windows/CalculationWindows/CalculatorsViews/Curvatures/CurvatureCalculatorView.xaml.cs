using System.Windows;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.Curvatures
{
    /// <summary>
    /// Логика взаимодействия для CurvatureCalculatorView.xaml
    /// </summary>
    public partial class CurvatureCalculatorView : Window
    {
        private CurvatureCalculatorViewModel viewModel;
        public CurvatureCalculatorView(CurvatureCalculatorViewModel viewModel)
        {
            InitializeComponent();
            viewModel.ParentWindow = this;
            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Refresh();
        }
    }
}
