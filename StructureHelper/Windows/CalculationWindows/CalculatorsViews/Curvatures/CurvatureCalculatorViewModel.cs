using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.Curvatures
{
    public class CurvatureCalculatorViewModel : OkCancelViewModelBase
    {
        private ICurvatureCalculator curvatureCalculator;
        public CalculatorViewModel CalculatorViewModel { get; private set; }
        public CurvatureCalculatorInputDataViewModel InputDataViewModel { get; private set; }

        public CurvatureCalculatorViewModel(ICurvatureCalculator calculator, ICrossSectionRepository repository)
        {
            this.curvatureCalculator = calculator;
            CalculatorViewModel = new CalculatorViewModel(calculator);
            InputDataViewModel = new(calculator.InputData, repository);
        }

        public void Refresh()
        {
            InputDataViewModel.Refresh();
        }
    }
}
