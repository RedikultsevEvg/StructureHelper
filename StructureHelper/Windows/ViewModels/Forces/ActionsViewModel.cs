using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.Forces;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ActionsViewModel : SelectItemVM<IForceAction>
    {
        readonly IUpdateStrategy<IAction> updateStrategy = new ActionUpdateStrategy();
        ICrossSectionRepository repository;
        private ICommand showCombinationsCommand;

        public ICommand ShowCombinations => showCombinationsCommand ?? (
            showCombinationsCommand = new RelayCommand(param =>
            {
                SafetyProcessor.RunSafeProcess(ShowDocumentMethod, "Error of opening of settings");
            }, o => SelectedItem is not null
            ));

        private void ShowDocumentMethod()
        {
            if (SelectedItem is null) { return; }
            var checkLogic = new CheckForceActionsLogic()
            {
                Entity = new List<IForceAction>() { SelectedItem }
            };
            if (checkLogic.Check() == false)
            {
                SafetyProcessor.ShowMessage($"Action {SelectedItem.Name} returned wrong collection of combinations", checkLogic.CheckResult);
                return;
            }
            var wnd = new ForceCombinationViewerView(SelectedItem);
            wnd.ShowDialog();
        }

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
                    NewItem = new ForceFactoredList() { Name = "New Factored Combination" };
                }
                else if (paramType == ActionType.ForceCombinationFromFile)
                {
                    NewItem = new ForceCombinationFromFile { Name = "New Combination from file" };
                }
                else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $": Actual type: {nameof(paramType)}");
                //GlobalRepository.Actions.Create(NewItem);
                base.AddMethod(parameter);
            }
        }

        public override void DeleteMethod(object parameter)
        {
            var dialogResult = MessageBox.Show("Delete action?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                DeleteAction();
                base.DeleteMethod(parameter);
            }         
        }

        public override void CopyMethod(object parameter)
        {
            NewItem = SelectedItem.Clone() as IForceAction;
            NewItem.Name = $"{NewItem.Name} copy";
            //GlobalRepository.Actions.Create(NewItem);
            Collection.Add(NewItem);
            Items.Add(NewItem);
            SelectedItem = NewItem;
        }

        public override void EditMethod(object parameter)
        {
            //var copyObject = GlobalRepository.Actions.GetById(SelectedItem.Id).Clone() as IAction;
            var copyObject = SelectedItem.Clone() as IAction;
            System.Windows.Window modelEditorWindow;
            if (SelectedItem is IForceCombinationList combinationList)
            {
                modelEditorWindow = new ForceCombinationView(combinationList);
            }
            else if (SelectedItem is IForceFactoredList factoredList)
            {
                modelEditorWindow = new ForceCombinationByFactorView(factoredList);
            }
            else if (SelectedItem is IForceCombinationFromFile fileCombination)
            {
                modelEditorWindow = new ForceCombinationFromFileView(fileCombination);
            }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $"actual object type: {nameof(SelectedItem)}");
            modelEditorWindow.ShowDialog();
            if (modelEditorWindow.DialogResult == true)
            {
                //GlobalRepository.Actions.Update(SelectedItem);
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

        private void DeleteAction()
        {
            var calcRepository = repository.Calculators;
            IHasForceActions forceCombinations;
            foreach (var calc in calcRepository)
            {
                if (calc is IForceCalculator forceCalculator)
                {
                    forceCombinations = forceCalculator.InputData;
                    forceCombinations.ForceActions.Remove(SelectedItem);
                }
                else if (calc is ICrackCalculator crackCalculator)
                {
                    forceCombinations = crackCalculator.InputData;
                    forceCombinations.ForceActions.Remove(SelectedItem);
                }
                else if (calc is ILimitCurvesCalculator)
                {
                    //nothing to do
                }
                else if (calc is IValueDiagramCalculator diagramCalculator)
                {
                    forceCombinations = diagramCalculator.InputData;
                    forceCombinations.ForceActions.Remove(SelectedItem);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calc));
                }
            }
        }
    }
}
