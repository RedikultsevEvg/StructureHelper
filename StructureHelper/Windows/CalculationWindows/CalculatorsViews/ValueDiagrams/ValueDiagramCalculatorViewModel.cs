using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ValueDiagrams
{
    public class ValueDiagramCalculatorViewModel : OkCancelViewModelBase
    {
        private IValueDiagramCalculator valueDiagramCalculator;

        public bool ShowTraceData
        {
            get => valueDiagramCalculator.ShowTraceData;
            set
            {
                valueDiagramCalculator.ShowTraceData = value;
                OnPropertyChanged(nameof(ShowTraceData));
            }
        }
        public string Name
        {
            get => valueDiagramCalculator.Name;
            set
            {
                valueDiagramCalculator.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public ValueDiagramCalculatorInputDataViewModel InputDataViewModel { get; set; }

        public ValueDiagramCalculatorViewModel(ICrossSectionRepository repository, IValueDiagramCalculator valueDiagramCalculator)
        {
            this.valueDiagramCalculator = valueDiagramCalculator;
            InputDataViewModel = new(repository, valueDiagramCalculator.InputData);
        }

        public void Refresh()
        {
            InputDataViewModel.Refresh();
        }
    }
}
