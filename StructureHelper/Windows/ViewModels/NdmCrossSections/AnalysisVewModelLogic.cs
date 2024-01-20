using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.ViewModels.Calculations.Calculators;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class AnalysisVewModelLogic : SelectItemVM<ICalculator>
    {
        private ICrossSectionRepository repository;
        private RelayCommand runCommand;
        static readonly CalculatorUpdateStrategy calculatorUpdateStrategy = new();

        public override void AddMethod(object parameter)
        {
            NewItem = new ForceCalculator() { Name = "New force calculator" };
            base.AddMethod(parameter);
        }
        public override void EditMethod(object parameter)
        {
            if (SelectedItem is ForceCalculator)
            {
                var calculator = SelectedItem as ForceCalculator;
                var calculatorCopy = (ICalculator)calculator.Clone();
                var vm = new ForceCalculatorViewModel(repository.Primitives, repository.ForceActions, calculator);

                var wnd = new ForceCalculatorView(vm);
                wnd.ShowDialog();
                if (wnd.DialogResult == true)
                {
                    // to do: update in repository
                }
                else
                {
                    calculatorUpdateStrategy.Update(calculator, calculatorCopy);
                }
            }
            base.EditMethod(parameter);
        }
        public override void DeleteMethod(object parameter)
        {
            var dialogResult = MessageBox.Show("Delete calculator?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                base.DeleteMethod(parameter);
            }      
        }
        public RelayCommand Run
        {
            get
            {
                return runCommand ??
                (
                runCommand = new RelayCommand(o =>
                {
                    SafetyProcessor.RunSafeProcess(RunCalculator, ErrorStrings.ErrorOfExuting + $": {SelectedItem.Name}");
                }, o => SelectedItem != null));
            }
        }

        private void RunCalculator()
        {
            SelectedItem.Run();
            var result = SelectedItem.Result;
            if (result.IsValid == false)
            {
                MessageBox.Show(result.Description, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var calculator = SelectedItem as ForceCalculator;
                var vm = new ForcesResultsViewModel(calculator);
                var wnd = new ForceResultsView(vm);
                wnd.ShowDialog();
            }
        }

        public AnalysisVewModelLogic(ICrossSectionRepository sectionRepository) : base(sectionRepository.CalculatorsList)
        {
            repository = sectionRepository;
        }
    }
}
