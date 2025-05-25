using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearActionsViewModel : SelectItemVM<IBeamShearAction>
    {
        private readonly IBeamShearRepository shearRepository;
        private IUpdateStrategy<IBeamShearAction> updateStrategy;

        public BeamShearActionsViewModel(IBeamShearRepository shearRepository) : base(shearRepository.Actions)
        {
            this.shearRepository = shearRepository;
        }
        public override void AddMethod(object parameter)
        {
            NewItem = BeamShearActionFactory.GetBeamShearAction(ShearActionTypes.DistributedLoad);
            base.AddMethod(parameter);
        }
        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            IBeamShearAction temporaryShearAction = SelectedItem.Clone() as IBeamShearAction;
            var window = new BeamShearActionView(SelectedItem);
            window.ShowDialog();
            if (window.DialogResult != true)
            {
                updateStrategy ??= new BeamShearActionUpdateStrategy();
                updateStrategy.Update(SelectedItem, temporaryShearAction);
            }
            base.EditMethod(parameter);
        }
        public override void DeleteMethod(object parameter)
        {
            var dialogResult = MessageBox.Show("Delete action?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                BeamShearRepositoryService.DeleteAction(shearRepository, SelectedItem);
                base.DeleteMethod(parameter);
            }
        }
    }
}
