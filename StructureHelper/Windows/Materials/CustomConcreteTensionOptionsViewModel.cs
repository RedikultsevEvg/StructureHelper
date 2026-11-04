using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Materials;
using System;
using System.ComponentModel;

namespace StructureHelper.Windows.ViewModels.Materials
{
    /// <summary>
    /// Child view model for the tension branch of a custom concrete diagram.
    /// LimitStrain is deliberately not exposed by the editor.
    /// </summary>
    public class CustomConcreteTensionOptionsViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly ICustomConcreteTensionOptions options;

        public CustomConcreteTensionOptionsViewModel(ICustomConcreteTensionOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public double Strength
        {
            get => options.Strength;
            set
            {
                if (options.Strength == value) return;
                options.Strength = value;
                OnPropertyChanged(nameof(Strength));
            }
        }

        public double UltimateStrain
        {
            get => options.UltimateStrain;
            set
            {
                if (options.UltimateStrain == value) return;
                options.UltimateStrain = value;
                OnPropertyChanged(nameof(UltimateStrain));
            }
        }

        public bool IsValid => string.IsNullOrEmpty(GetError(nameof(Strength)))
            && string.IsNullOrEmpty(GetError(nameof(UltimateStrain)));

        public string Error => string.Empty;

        public string this[string columnName] => GetError(columnName);

        private string GetError(string columnName)
        {

            if (columnName == nameof(Strength))
            {
                if (!IsFinite(Strength) || Strength <= 0) return "Strength must be positive when tension work is enabled";
            }
            else if (columnName == nameof(UltimateStrain))
            {
                if (!IsFinite(UltimateStrain) || UltimateStrain <= 0) return "Ultimate strain must be positive when tension work is enabled";
            }
            return string.Empty;
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(Strength));
            OnPropertyChanged(nameof(UltimateStrain));
        }

        private static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    }
}
