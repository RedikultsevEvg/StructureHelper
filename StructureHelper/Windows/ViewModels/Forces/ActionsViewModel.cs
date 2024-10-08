using StructureHelper.Infrastructure.Enums;
using StructureHelper.Services.Settings;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using System.Windows.Forms;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ActionsViewModel : SelectItemVM<IForceAction>
    {
        readonly IUpdateStrategy<IAction> updateStrategy = new ActionUpdateStrategy();
        ICrossSectionRepository repository;

        public override void AddMethod(object parameter)
        {
            if (parameter is not null)
            {
                var paramType = (ActionType)parameter;
                if (paramType == ActionType.ForceCombination)
                {
                    NewItem = new ForceCombinationList() { Name = "New Force Combination" };
                }
                else if (paramType == ActionType.ForceCombinationByFactor)
                {
                    NewItem = new ForceCombinationByFactor() { Name = "New Factored Combination" };
                }
                else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $": Actual type: {nameof(paramType)}");
                GlobalRepository.Actions.Create(NewItem);
                base.AddMethod(parameter);
            }
        }

        public override void DeleteMethod(object parameter)
        {
            var dialogResult = MessageBox.Show("Delete action?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                if (DeleteAction() != true) return;
                GlobalRepository.Actions.Delete(SelectedItem.Id);
                base.DeleteMethod(parameter);
            }         
        }

        public override void CopyMethod(object parameter)
        {
            NewItem = SelectedItem.Clone() as IForceAction;
            NewItem.Name = $"{NewItem.Name} copy";
            GlobalRepository.Actions.Create(NewItem);
            Collection.Add(NewItem);
            Items.Add(NewItem);
            SelectedItem = NewItem;
        }

        public override void EditMethod(object parameter)
        {
            var copyObject = GlobalRepository.Actions.GetById(SelectedItem.Id).Clone() as IAction;
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
            if (wnd.DialogResult == true)
            {
                GlobalRepository.Actions.Update(SelectedItem);
            }
            else
            {   
                updateStrategy.Update(SelectedItem, copyObject);
            }
            base.EditMethod(parameter);
        }

        public ActionsViewModel(ICrossSectionRepository repository) : base (repository.ForceActions)
        {
            this.repository = repository;
        }

        private bool DeleteAction()
        {
            bool result = true;
            var calcRepository = repository.Calculators;
            foreach (var calc in calcRepository)
            {
                if (calc is ForceCalculator forceCalculator)
                {
                    var forceCombinations = forceCalculator.InputData as IHasForceCombinations;
                    result = DeleteActionFromHost(result, calc, forceCombinations);
                }
                else if (calc is CrackCalculator calculator)
                {
                    var forceCombinations = calculator.InputData as IHasForceCombinations;
                    result = DeleteActionFromHost(result, calc, forceCombinations);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calc));
                }
            }
            return result;
        }

        private bool DeleteActionFromHost(bool result, ICalculator item, IHasForceCombinations? forceCombinations)
        {
            var containSelected = forceCombinations.ForceActions.Contains(SelectedItem);
            if (containSelected)
            {
                var dialogResultCalc = MessageBox.Show($"Action is contained in calculator {item.Name}", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResultCalc == DialogResult.Yes)
                {
                    forceCombinations.ForceActions.Remove(SelectedItem);
                }
                else result = false;
            }

            return result;
        }
    }
}
