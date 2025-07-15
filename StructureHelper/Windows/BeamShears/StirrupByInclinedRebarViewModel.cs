using StructureHelper.Windows.MainWindow.Materials;
using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelper.Windows.BeamShears
{
    public class StirrupByInclinedRebarViewModel : OkCancelViewModelBase
    {
        private readonly IStirrupByInclinedRebar stirrupByInclinedRebar;


        public string Name
        {
            get => stirrupByInclinedRebar.Name;
            set
            {
                stirrupByInclinedRebar.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public double CompressedGap
        {
            get => stirrupByInclinedRebar.CompressedGap;
            set
            {
                stirrupByInclinedRebar.CompressedGap = value;
                OnPropertyChanged(nameof(CompressedGap));
            }
        }

        public double StartCoordinate
        {
            get => stirrupByInclinedRebar.StartCoordinate;
            set
            {
                stirrupByInclinedRebar.StartCoordinate = value;
                OnPropertyChanged(nameof(StartCoordinate));
            }
        }

        public double AngleOfInclination
        {
            get => stirrupByInclinedRebar.AngleOfInclination;
            set
            {
                stirrupByInclinedRebar.AngleOfInclination = value;
                OnPropertyChanged(nameof(AngleOfInclination));
            }
        }

        public double OffSet
        {
            get => stirrupByInclinedRebar.OffSet;
            set
            {
                stirrupByInclinedRebar.OffSet = value;
                OnPropertyChanged(nameof(OffSet));
            }
        }

        public double LegCount
        {
            get => stirrupByInclinedRebar.LegCount;
            set
            {
                if (value < 0.0) { value = 0.0; }
                stirrupByInclinedRebar.LegCount = value;
                OnPropertyChanged(nameof(LegCount));
            }
        }

        public RebarSectionViewModel RebarSectionViewModel {get;}

        public StirrupByInclinedRebarViewModel(IStirrupByInclinedRebar stirrupByInclinedRebar)
        {
            this.stirrupByInclinedRebar = stirrupByInclinedRebar;
            RebarSectionViewModel = new(this.stirrupByInclinedRebar.RebarSection)
            {
                MinRebarDiameter = 0.003,
                MaxRebarDiameter = 0.032
            };
        }
    }
}
