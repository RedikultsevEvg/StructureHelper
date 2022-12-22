using StructureHelper.Infrastructure;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.CrossSections;
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

        public IForceCombinationList SelectedItem { get; set; }

        public ObservableCollection<IForceCombinationList> Items { get; private set; }

        private RelayCommand addForceCombinationCommand;
        public RelayCommand Add
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
            repository.ForceCombinationLists.Add(item);
        }
        private RelayCommand deleteForceCombinationCommand;
        public RelayCommand Delete
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
                repository.ForceCombinationLists.Remove(SelectedItem);
            }
        }
        private RelayCommand editForceCombinationCommand;
        public RelayCommand Edit
        {
            get
            {
                return editForceCombinationCommand ??
                    (
                    editForceCombinationCommand = new RelayCommand(o =>
                    {
                        EditForceCombination();
                        OnPropertyChanged(nameof(Items));
                    }, o => SelectedItem != null));
            }
        }

        public RelayCommand Copy => throw new NotImplementedException();

        private void EditForceCombination()
        {
            var wnd = new ForceCombinationView(SelectedItem);
            wnd.ShowDialog();
        }

        public void AddItems(IEnumerable<IForceCombinationList> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public ForceCombinationViewModelLogic(ICrossSectionRepository repository)
        { 
            this.repository = repository;
            Items = new ObservableCollection<IForceCombinationList>();
            AddItems(this.repository.ForceCombinationLists);
        }
    }
}
