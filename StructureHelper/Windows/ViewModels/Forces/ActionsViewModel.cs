using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
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
    public class ActionsViewModel : CRUDViewModelBase<IForceAction>
    {
        ICrossSectionRepository repository;

        public override void AddMethod(object parameter)
        {
            if (parameter is not null)
            {
                ActionType paramType = (ActionType)parameter;
                if (paramType == ActionType.ForceCombination)
                {
                    NewItem = new ForceCombinationList() { Name = "New Force Combination" };
                }
                else if (paramType == ActionType.ForceCombinationByFactor)
                {
                    NewItem = new ForceCombinationByFactor() { Name = "New Factored Combination" };
                }
                else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $": Actual type: {nameof(paramType)}");    
                base.AddMethod(parameter);
            }

        }

        public override void DeleteMethod(object parameter)
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
                        var containSelected = forceCalculator.ForceActions.Contains(SelectedItem);
                        if (containSelected)
                        {
                            var dialogResultCalc = MessageBox.Show($"Action is contained in calculator {item.Name}", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialogResultCalc == DialogResult.OK) { forceCalculator.ForceActions.Remove(SelectedItem); }
                            else return;
                        }
                    }
                }
                base.DeleteMethod(parameter);
            }         
        }

        public override void EditMethod(object parameter)
        {
            System.Windows.Window wnd;
            if (SelectedItem is IForceCombinationList)
            {
                var item = (IForceCombinationList)SelectedItem;
                wnd = new ForceCombinationView(item);
            }
            else if (SelectedItem is IForceCombinationByFactor)
            {
                var item = (IForceCombinationByFactor)SelectedItem;
                wnd = new ForceCombinationByFactorView(item);
            }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $"actual object type: {nameof(SelectedItem)}");
            wnd.ShowDialog();
            base.EditMethod(parameter);
        }

        public ActionsViewModel(ICrossSectionRepository repository) : base (repository.ForceActions)
        {
            this.repository = repository;
        }
    }
}
