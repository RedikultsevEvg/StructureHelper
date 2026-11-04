using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Materials;
using System;

namespace StructureHelper.Windows.ViewModels.Materials
{
    /// <summary>
    /// Read-only child view model used in the custom concrete material list.
    /// </summary>
    public class CustomConcreteModelItemViewModel : ViewModelBase
    {
        public ICustomConcreteModel Model { get; }

        public CustomConcreteModelItemViewModel(ICustomConcreteModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public bool IsActive => Model.IsActive;
        public string Name => Model.Name;
        public string LimitState => Model.StateCalcTermPair.LimitState.ToString();
        public string CalcTerm => Model.StateCalcTermPair.CalcTerm.ToString();
        public double YoungModulus => Model.YoungModulus;
        public double PeakStress => Model.CompressionOptions.PeakStress;
        public double Strength => Model.TensionOptions.Strength;

        public void Refresh()
        {
            OnPropertyChanged(nameof(IsActive));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(LimitState));
            OnPropertyChanged(nameof(CalcTerm));
            OnPropertyChanged(nameof(YoungModulus));
            OnPropertyChanged(nameof(PeakStress));
            OnPropertyChanged(nameof(Strength));
        }
    }
}
