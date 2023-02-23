using StructureHelper.Windows.Forces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ActionsViewModel : CRUDViewModelBase<IForceCombinationList>
    {
        ICrossSectionRepository repository;

        public override void AddMethod(object parameter)
        {
            NewItem = new ForceCombinationList() { Name = "New Force Combination" };
            base.AddMethod(parameter);
        }

        public override void DeleteMethod()
        {
            var dialogResult = MessageBox.Show("Delete action?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                var calcRepository = repository.CalculatorsList;
                foreach (var item in calcRepository)
                {
                    if (item is IForceCalculator)
                    {
                        var forceCalculator = item as IForceCalculator;
                        var containSelected = forceCalculator.ForceCombinationLists.Contains(SelectedItem);
                        if (containSelected)
                        {
                            var dialogResultCalc = MessageBox.Show($"Action is contained in calculator {item.Name}", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialogResultCalc == DialogResult.OK) { forceCalculator.ForceCombinationLists.Remove(SelectedItem); }
                            else return;
                        }
                    }
                }
                base.DeleteMethod();
            }         
        }

        public override void EditMethod(object parameter)
        {
            var wnd = new ForceCombinationView(SelectedItem);
            wnd.ShowDialog();
            base.EditMethod(parameter);
        }

        public ActionsViewModel(ICrossSectionRepository repository) : base (repository.ForceCombinationLists)
        {
            this.repository = repository;
        }
    }
}
