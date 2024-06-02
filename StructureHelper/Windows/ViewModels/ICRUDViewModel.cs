using StructureHelper.Infrastructure;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels
{
    public interface ICRUDViewModel<TItem>
    {
        TItem SelectedItem { get; set; }
        ObservableCollection<TItem> Items { get; }
        ICommand Add { get; }
        ICommand Delete { get; }
        ICommand Edit { get; }
        ICommand Copy { get; }
        void AddItems(IEnumerable<TItem> items);
    }
}
