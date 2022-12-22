using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using StructureHelper.Windows.ViewModels.Calculations.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class CalculatorsViewModelLogic : ViewModelBase, ICalculatorsViewModelLogic
    {
        private readonly ICrossSectionRepository repository;

        public INdmCalculator SelectedItem { get; set; }
        public ObservableCollection<INdmCalculator> Items { get; private set; }

        private RelayCommand addCalculatorCommand;
        public RelayCommand Add
        {
            get
            {
                return addCalculatorCommand ??
                    (
                    addCalculatorCommand = new RelayCommand(o =>
                    {
                        AddCalculator();
                        OnPropertyChanged(nameof(Items));
                    }));
            }
        }
        private void AddCalculator()
        {
            var item = new ForceCalculator() { Name = "New force calculator" };
            Items.Add(item);
            repository.CalculatorsList.Add(item);
        }
        private RelayCommand editCalculatorCommand;
        public RelayCommand Edit
        {
            get
            {
                return editCalculatorCommand ??
                    (
                    editCalculatorCommand = new RelayCommand(o =>
                    {
                        var tmpSelected = SelectedItem;
                        EditCalculator();
                        Items.Clear();
                        AddItems(repository.CalculatorsList);
                        OnPropertyChanged(nameof(Items));
                        SelectedItem = tmpSelected;
                    }, o => SelectedItem != null));
            }
        }
        private void EditCalculator()
        {
            if (SelectedItem is ForceCalculator)
            {
                var calculator = SelectedItem as ForceCalculator;
                var vm = new ForceCalculatorViewModel(repository.Primitives, repository.ForceCombinationLists, calculator);

                var wnd = new ForceCalculatorView(vm);
                wnd.ShowDialog();
            }
        }
        private RelayCommand deleteCalculatorCommand;
        private RelayCommand runCommand;
        private RelayCommand copyCalculatorCommand;

        public RelayCommand Delete
        {
            get
            {
                return deleteCalculatorCommand ??
                    (
                    deleteCalculatorCommand = new RelayCommand(o =>
                    {
                        DeleteCalculator();
                    }, o => SelectedItem != null));
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

        public RelayCommand Copy
        {
            get
            {
                return copyCalculatorCommand ??
                (
                copyCalculatorCommand = new RelayCommand(o =>
                {
                    var item = SelectedItem.Clone() as INdmCalculator;
                    repository.CalculatorsList.Add(item);
                    Items.Add(item);
                    OnPropertyChanged(nameof(Items));
                }, o => SelectedItem != null));
            }
        }

        private void DeleteCalculator()
        {
            var dialogResult = MessageBox.Show("Delete calculator?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                repository.CalculatorsList.Remove(SelectedItem as INdmCalculator);
                OnPropertyChanged(nameof(Items));
            }
        }

        public void AddItems(IEnumerable<INdmCalculator> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public CalculatorsViewModelLogic(ICrossSectionRepository repository)
        {
            this.repository = repository;
            Items = new ObservableCollection<INdmCalculator>();
            AddItems(this.repository.CalculatorsList);
        }
    }
}
