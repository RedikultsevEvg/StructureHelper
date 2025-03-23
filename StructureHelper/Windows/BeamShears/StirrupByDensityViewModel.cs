using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class StirrupByDensityViewModel : OkCancelViewModelBase
    {
        private readonly IStirrupByDensity stirrupByDensity;

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

        public StirrupByDensityViewModel(IStirrupByDensity stirrupByDensity)
        {
            this.stirrupByDensity = stirrupByDensity;
        }
    }
}
