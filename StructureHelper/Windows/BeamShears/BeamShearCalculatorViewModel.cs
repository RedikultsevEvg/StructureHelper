using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearCalculatorViewModel : OkCancelViewModelBase
    {
        private readonly IBeamShearRepository shearRepository;
        private readonly IBeamShearCalculator calculator;
        private bool showTraceData;

        public string Name
        {
            get => calculator.Name;
            set
            {
                calculator.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public bool ShowTraceData
        {
            get
            {
                return calculator.ShowTraceData;
            }
            set
            {
                calculator.ShowTraceData = value;
                OnPropertyChanged(nameof(ShowTraceData));
            }
        }
        public BeamShearCalculatorInputDataViewModel InputDataViewModel { get; }
        public BeamShearCalculatorViewModel(IBeamShearRepository shearRepository, IBeamShearCalculator calculator)
        {
            this.shearRepository = shearRepository;
            this.calculator = calculator;
            InputDataViewModel = new(this.shearRepository, this.calculator.InputData);
        }

        internal void Refresh()
        {
            InputDataViewModel.Refresh();
        }
    }
}
