using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ValueDiagrams
{
    public class ValueDiagramCalculatorViewModel : OkCancelViewModelBase
    {
        private IValueDiagramCalculator valueDiagramCalculator;
        
        public CalculatorViewModel CalculatorViewModel { get; private set; }
        public ValueDiagramCalculatorInputDataViewModel InputDataViewModel { get; set; }

        public ValueDiagramCalculatorViewModel(ICrossSectionRepository repository, IValueDiagramCalculator valueDiagramCalculator)
        {
            this.valueDiagramCalculator = valueDiagramCalculator;
            CalculatorViewModel = new(valueDiagramCalculator);
            InputDataViewModel = new(valueDiagramCalculator.InputData, repository);
        }

        public void Refresh()
        {
            InputDataViewModel.Refresh();
        }
    }
}
