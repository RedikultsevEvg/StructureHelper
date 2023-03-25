using StructureHelper.Infrastructure;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class ForceCombinationViewModelLogic : ViewModelBase, IForceCombinationViewModelLogic
    {
        private readonly ICrossSectionRepository repository;

        public IForceAction SelectedItem { get; set; }

        public ObservableCollection<IForceAction> Items { get; private set; }

        private ICommand addForceCombinationCommand;
        public ICommand Add
        {
            get
            {
                return addForceCombinationCommand ??
                    (
                    addForceCombinationCommand = new RelayCommand(o =>
                    {
                        AddCombination();
                        OnPropertyChanged(nameof(Items));
                    }));
            }
        }
        private void AddCombination()
        {
            var item = new ForceCombinationList() { Name = "New Force Combination" };
            Items.Add(item);
            repository.ForceActions.Add(item);
        }
        private ICommand deleteForceCombinationCommand;
        public ICommand Delete
        {
            get
            {
                return deleteForceCombinationCommand ??
                    (
                    deleteForceCombinationCommand = new RelayCommand(o =>
                    {
                        DeleteForceCombination();
                        OnPropertyChanged(nameof(Items));
                    }, o => SelectedItem != null));
            }
        }
        private void DeleteForceCombination()
        {
            var dialogResult = MessageBox.Show("Delete action?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                repository.ForceActions.Remove(SelectedItem);
            }
        }
        private ICommand editForceCombinationCommand;
        private ICommand copyCommand;

        public ICommand Edit
        {
            get
            {
                return editForceCombinationCommand ??
                    (
                    editForceCombinationCommand = new RelayCommand(o =>
                    {
                        EditForceCombination();
                        Items.Clear();
                        AddItems(repository.ForceActions);
                        OnPropertyChanged(nameof(Items));
                    }, o => SelectedItem != null));
            }
        }

        public ICommand Copy
        {
            get
            {
                return copyCommand ??
                (
                copyCommand = new RelayCommand(o =>
                {
                    var item = SelectedItem.Clone() as IForceCombinationList;
                    repository.ForceActions.Add(item);
                    Items.Add(item);
                    OnPropertyChanged(nameof(Items));
                }, o => SelectedItem != null));
            }
        }

        private void EditForceCombination()
        {
            //var wnd = new ForceCombinationView(SelectedItem);
            //wnd.ShowDialog();
        }

        public void AddItems(IEnumerable<IForceAction> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public ForceCombinationViewModelLogic(ICrossSectionRepository repository)
        { 
            this.repository = repository;
            Items = new ObservableCollection<IForceAction>();
            AddItems(this.repository.ForceActions);
        }
    }
}
