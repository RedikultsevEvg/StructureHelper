using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperLogics.Models.BeamShears;
using System.ComponentModel;

namespace StructureHelper.Windows.BeamShears
{
    public class StirrupByRebarViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private readonly IStirrupByRebar stirrupByRebar;

        public string Name
        {
            get => stirrupByRebar.Name;
            set
            {
                stirrupByRebar.Name = value;
            }
        }

        public double Diameter
        {
            get => stirrupByRebar.Diameter;
            set
            {
                if (value < 0)
                {
                    value = 0.008;
                }
                stirrupByRebar.Diameter = value;
                OnPropertyChanged(nameof(Diameter));
            }
        }

        public double LegCount
        {
            get => stirrupByRebar.LegCount;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                stirrupByRebar.LegCount = value;
                OnPropertyChanged(nameof(LegCount));
            }
        }

        public double Spacing
        {
            get => stirrupByRebar.Spacing;
            set
            {
                if (value < 0)
                {
                    value = 0.1;
                }
                stirrupByRebar.Spacing = value;
                OnPropertyChanged(nameof(Spacing));
            }
        }

        public bool IsSpiral
        {
            get => stirrupByRebar.IsSpiral;
            set
            {
                stirrupByRebar.IsSpiral = value;
                OnPropertyChanged(nameof(IsSpiral));
            }
        }

        public ReinforcementViewModel Material { get; private set; }

        public string Error => null;

        public double MinDiameter { get; set; } = 0.003;
        public double MinLegCount { get; set; } = 0;
        /// <summary>
        /// Minimum value of spacing in meters
        /// </summary>
        public double MinSpacing { get; set; } = 0.02;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == nameof(Diameter))
                {
                    if (Diameter < MinDiameter)
                    {
                        result = $"Diameter of stirrups must not be less than {MinDiameter}(m)";
                    }
                }
                if (columnName == nameof(LegCount))
                {
                    if (LegCount < MinLegCount)
                    {
                        result = $"Number of legs of stirrups must not be less than {MinLegCount}";
                    }
                }
                if (columnName == nameof(Spacing))
                {
                    if (Spacing < MinSpacing)
                    {
                        result = $"Spacing of stirrups must not be less than {MinSpacing}";
                    }
                }
                return result;
            }
        }

        public StirrupByRebarViewModel(IStirrupByRebar stirrupByRebar)
        {
            this.stirrupByRebar = stirrupByRebar;
            Material = new(this.stirrupByRebar.Material) { MaterialLogicVisibility = false};
        }
    }
}
