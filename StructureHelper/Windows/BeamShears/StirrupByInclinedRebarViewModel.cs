using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public StirrupByInclinedRebarViewModel(IStirrupByInclinedRebar stirrupByInclinedRebar)
        {
            this.stirrupByInclinedRebar = stirrupByInclinedRebar;
        }
    }
}
