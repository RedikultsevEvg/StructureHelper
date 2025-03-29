using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelper.Windows.BeamShears
{
    public class StirrupByRebarViewModel : OkCancelViewModelBase
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

        public ReinforcementViewModel Material { get; private set; }

        public StirrupByRebarViewModel(IStirrupByRebar stirrupByRebar)
        {
            this.stirrupByRebar = stirrupByRebar;
            Material = new(this.stirrupByRebar.Material) { MaterialLogicVisibility = false};
        }
    }
}
