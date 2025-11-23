using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using System.Windows;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.Curvatures
{
    /// <summary>
    /// Логика взаимодействия для CurvatureCalculatorResultView.xaml
    /// </summary>
    public partial class CurvatureCalculatorResultView : Window
    {
        private readonly CurvatureCalculatorResultViewModel viewModel;
        public CurvatureCalculatorResultView(CurvatureCalculatorResultViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }

        public CurvatureCalculatorResultView(ICurvatureCalculatorResult? curvatureResult) : this(new CurvatureCalculatorResultViewModel(curvatureResult))
        {
        }
    }
}
