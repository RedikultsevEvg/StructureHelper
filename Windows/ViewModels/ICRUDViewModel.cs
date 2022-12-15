using StructureHelper.Infrastructure;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels
{
    public interface ICRUDViewModel<TItem>
    {
        TItem SelectedItem { get; set; }
        ObservableCollection<TItem> Items { get; }
        RelayCommand Add { get; }
        RelayCommand Delete { get; }
        RelayCommand Edit { get; }
    }
}
