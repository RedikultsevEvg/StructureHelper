using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Materials;
using System;
using System.ComponentModel;

namespace StructureHelper.Windows.ViewModels.Materials
{
    /// <summary>
    /// Child view model for the compression branch of a custom concrete diagram.
    /// </summary>
    public class CustomConcreteCompressionOptionsViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly ICustomConcreteCompressionOptions options;

        public CustomConcreteCompressionOptionsViewModel(ICustomConcreteCompressionOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public double PeakStress
        {
            get => options.PeakStress;
            set
            {
                if (options.PeakStress == value) return;
                options.PeakStress = value;
                OnPropertyChanged(nameof(PeakStress));
            }
        }

        public double PeakStrain
        {
            get => options.PeakStrain;
            set
            {
                if (options.PeakStrain == value) return;
                options.PeakStrain = value;
                OnPropertyChanged(nameof(PeakStrain));
            }
        }

        public double LimitStrain
        {
            get => options.LimitStrain;
            set
            {
                if (options.LimitStrain == value) return;
                options.LimitStrain = value;
                OnPropertyChanged(nameof(LimitStrain));
            }
        }

        public double DescendingScaleFactor
        {
            get => options.DescendingScaleFactor;
            set
            {
                if (options.DescendingScaleFactor == value) return;
                options.DescendingScaleFactor = value;
                OnPropertyChanged(nameof(DescendingScaleFactor));
            }
        }

        public bool IsValid => string.IsNullOrEmpty(GetError(nameof(PeakStress)))
            && string.IsNullOrEmpty(GetError(nameof(PeakStrain)))
            && string.IsNullOrEmpty(GetError(nameof(LimitStrain)))
            && string.IsNullOrEmpty(GetError(nameof(DescendingScaleFactor)));

        public string Error => string.Empty;

        public string this[string columnName] => GetError(columnName);

        private string GetError(string columnName)
        {
            if (columnName == nameof(PeakStress))
            {
                if (!IsFinite(PeakStress) || PeakStress <= 0) return "Peak stress must be positive";
            }
            else if (columnName == nameof(PeakStrain))
            {
                if (!IsFinite(PeakStrain) || PeakStrain <= 0) return "Peak strain must be positive";
            }
            else if (columnName == nameof(LimitStrain))
            {
                if (!IsFinite(LimitStrain) || LimitStrain <= PeakStrain) return "Limit strain must be greater than peak strain";
            }
            else if (columnName == nameof(DescendingScaleFactor))
            {
                if (!IsFinite(DescendingScaleFactor) || DescendingScaleFactor <= 0) return "Descending scale factor must be positive";
            }
            return string.Empty;
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(PeakStress));
            OnPropertyChanged(nameof(PeakStrain));
            OnPropertyChanged(nameof(LimitStrain));
            OnPropertyChanged(nameof(DescendingScaleFactor));
        }

        private static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    }
}
