using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.ViewModels.Calculations.Calculators;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class AnalysisVewModel : CRUDViewModelBase<INdmCalculator>
    {
        private ICrossSectionRepository repository;
        private RelayCommand runCommand;

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
                var vm = new ForceCalculatorViewModel(repository.Primitives, repository.ForceCombinationLists, calculator);

                var wnd = new ForceCalculatorView(vm);
                wnd.ShowDialog();
            }
            base.EditMethod(parameter);
        }
        public override void DeleteMethod()
        {
            var dialogResult = MessageBox.Show("Delete calculator?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                base.DeleteMethod();
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
                    SelectedItem.Run();
                    var result = SelectedItem.Result;
                    if (result.IsValid == false)
                    {
                        MessageBox.Show(result.Desctription, "Check data for analisys", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var calculator = SelectedItem as IForceCalculator;
                        var vm = new ForcesResultsViewModel(calculator);
                        var wnd = new ForceResultsView(vm);
                        wnd.ShowDialog();
                    }
                }, o => SelectedItem != null));
            }
        }
        public AnalysisVewModel(ICrossSectionRepository sectionRepository) : base(sectionRepository.CalculatorsList)
        {
            repository = sectionRepository;
        }
    }
}
