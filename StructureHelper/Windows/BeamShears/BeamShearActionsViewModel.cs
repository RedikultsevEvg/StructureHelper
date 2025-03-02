using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearActionsViewModel : SelectItemVM<IBeamShearAction>
    {
        private readonly IBeamShearRepository shearRepository;

        public BeamShearActionsViewModel(IBeamShearRepository shearRepository) : base(shearRepository.BeamShearActions)
        {
            this.shearRepository = shearRepository;
        }
        public override void DeleteMethod(object parameter)
        {
            shearRepository.DeleteAction(SelectedItem);
            base.DeleteMethod(parameter);
        }
    }
}
