using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StructureHelper.Windows.ViewModels
{
    public class SourceToTargetViewModel<TItem> : ViewModelBase, ISourceToTargetViewModel<TItem>
    {
        private IEnumerable<TItem> allowedItems;
        private IEnumerable<TItem> targetItems;
        private RelayCommand addAll;
        private RelayCommand addSelected;
        private RelayCommand removeSelected;
        private RelayCommand clearAll;

        public TItem SelectedSourceItem { get; set; }
        public TItem SelectedTargetItem { get; set; }
        public ObservableCollection<TItem> SourceItems { get; }
        public ObservableCollection<TItem> TargetItems { get; }
        public DataTemplate ItemDataDemplate { get; set; }
        public RelayCommand AddAll
        {
            get
            {
                return addAll ??
                    (
                    addAll = new RelayCommand(o =>
                    {
                        foreach (var item in SourceItems)
                        {
                            TargetItems.Add(item);
                        }
                        SourceItems.Clear();
                    }, o=> SourceItems.Count() > 0));
            }
        }
        public RelayCommand ClearAll
        {
            get
            {
                return clearAll ??
                    (
                    clearAll = new RelayCommand(o =>
                    {
                        foreach (var item in TargetItems)
                        {
                            SourceItems.Add(item);
                        }
                        TargetItems.Clear();
                    }, o => TargetItems.Count() > 0));
            }
        }
        public RelayCommand AddSelected
        {
            get
            {
                return addSelected ??
                    (
                    addSelected = new RelayCommand(o =>
                    {
                        TargetItems.Add(SelectedSourceItem);
                        SourceItems.Remove(SelectedSourceItem);
                    }, o => SelectedSourceItem != null));
            }
        }
        public RelayCommand RemoveSelected
        {
            get
            {
                return removeSelected ??
                    (
                    removeSelected = new RelayCommand(o =>
                    {
                        SourceItems.Add(SelectedTargetItem);
                        TargetItems.Remove(SelectedTargetItem);
                    }, o => SelectedTargetItem != null));
            }
        }

        

        public SourceToTargetViewModel()
        {
            SourceItems = new ObservableCollection<TItem>();
            TargetItems = new ObservableCollection<TItem>();
        }

        public void SetSourceItems(IEnumerable<TItem> allowedItems)
        {
            this.allowedItems = allowedItems;
            var rejectedItems = allowedItems.Where(x => TargetItems.Contains(x));
            var filteredItems = allowedItems.Except(rejectedItems);
            SourceItems.Clear();
            foreach (var item in filteredItems)
            {
                SourceItems.Add(item);
            }
        }
        public void  SetTargetItems(IEnumerable<TItem> targetItems)
        {
            this.targetItems = targetItems;
            TargetItems.Clear();
            foreach (var item in this.targetItems)
            {
                TargetItems.Add(item);
            }
        }
        public IEnumerable<TItem> GetTargetItems()
        {
            return TargetItems;
        }

    }
}
