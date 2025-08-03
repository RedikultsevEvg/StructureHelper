using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;
using System.ComponentModel;

namespace StructureHelper.Windows.BeamShears
{
    public class StirrupByDensityViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private readonly IStirrupByDensity stirrupByDensity;
        public double MinDensity { get; set; } = 0;

        public string Name
        {
            get => stirrupByDensity.Name;
            set
            {
                stirrupByDensity.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public double Density
        {
            get => stirrupByDensity.StirrupDensity;
            set
            {
                stirrupByDensity.StirrupDensity = value;
                OnPropertyChanged(nameof(Density));
            }
        }

        public double StartCoordinate
        {
            get => stirrupByDensity.StartCoordinate;
            set
            {
                if (value < 0) { value = 0; }
                stirrupByDensity.StartCoordinate = value;
                OnPropertyChanged(nameof(StartCoordinate));
            }
        }

        public double EndCoordinate
        {
            get => stirrupByDensity.EndCoordinate;
            set
            {
                if (value < 0) { value = 0; }
                stirrupByDensity.EndCoordinate = value;
                OnPropertyChanged(nameof(EndCoordinate));
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == nameof(Density))
                {
                    if (Density < MinDensity)
                    {
                        result = $"Density of stirrups must not be less than {MinDensity}(N/m)";
                    }
                }
                return result;
            }
        }

        public StirrupByDensityViewModel(IStirrupByDensity stirrupByDensity)
        {
            this.stirrupByDensity = stirrupByDensity;
        }
    }
}
