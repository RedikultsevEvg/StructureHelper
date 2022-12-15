using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels
{
    public interface ISourceToTargetViewModel<TItem>
    {
        TItem SelectedSourceItem { get; set; }
        TItem SelectedTargetItem { get; set; }
        ObservableCollection<TItem> SourceItems { get; }
        ObservableCollection<TItem> TargetItems { get; }
        RelayCommand AddAll { get; }
        RelayCommand ClearAll { get; }
        RelayCommand AddSelected { get; }
        RelayCommand RemoveSelected { get; }
        void SetSourceItems(IEnumerable<TItem> allowedItems);
        void SetTargetItems(IEnumerable<TItem> targetItems);
        IEnumerable<TItem> GetTargetItems();
    }
}
