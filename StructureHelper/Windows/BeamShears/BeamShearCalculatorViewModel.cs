using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearCalculatorViewModel : OkCancelViewModelBase
    {
        private readonly IBeamShearRepository shearRepository;
        private readonly IBeamShearCalculator calculator;
        

        public string Name
        {
            get => calculator.Name;
            set
            {
                calculator.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public BeamShearCalculatorInputDataViewModel InputDataViewModel { get; }
        public BeamShearCalculatorViewModel(IBeamShearRepository shearRepository, IBeamShearCalculator calculator)
        {
            this.shearRepository = shearRepository;
            this.calculator = calculator;
            InputDataViewModel = new(this.shearRepository, this.calculator.InputData);
        }
    }
}
