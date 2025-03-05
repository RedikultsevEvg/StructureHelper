using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.TreeGraph
{
    public class RenameViewModel : ViewModelBase
    {

        private RelayCommand saveCommand;
        public ICommand SaveCommand
        {
            get => saveCommand ??= new RelayCommand(o => Save(o));
        }
        private string functionName;
        public string FunctionName
        {
            get => functionName;
            set
            {
                functionName = value;
                OnPropertyChanged(nameof(FunctionName));
            }
        }
        public RenameViewModel(TreeViewItemViewModel item)
        {
            FunctionName = item.Name;
        }
        private void Save(object parameter)
        {
            var window = parameter as Window;
            window.DialogResult = true;
            window.Close();
        }
    }
}
