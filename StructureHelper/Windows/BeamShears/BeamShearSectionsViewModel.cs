using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearSectionsViewModel : SelectItemVM<IBeamShearSection>
    {
        IBeamShearRepository shearRepository;
        public BeamShearSectionsViewModel(IBeamShearRepository shearRepository) : base(shearRepository.ShearSections)
        {
            this.shearRepository = shearRepository;
        }
        public override void DeleteMethod(object parameter)
        {
            shearRepository.DeleteSection(SelectedItem);
            base.DeleteMethod(parameter);
        }
    }
}
