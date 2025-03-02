using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamStirrupsViewModel : SelectItemVM<IStirrup>
    {
        private readonly IBeamShearRepository shearRepository;

        public BeamStirrupsViewModel(IBeamShearRepository shearRepository) : base(shearRepository.Stirrups)
        {
            this.shearRepository = shearRepository;
        }
        public override void DeleteMethod(object parameter)
        {
            shearRepository.DeleteStirrup(SelectedItem);
            base.DeleteMethod(parameter);
        }
    }
}
