using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels
{
    public abstract class OkCancelViewModelBase : ViewModelBase
    {
        public Window ParentWindow { get; set; }
        public ICommand OkCommand => new RelayCommand(o => OkAction());
        public ICommand CancelCommand => new RelayCommand(o => CancelAction());
        private void CancelAction()
        {
            ParentWindow.DialogResult = false;
            ParentWindow.Close();
        }

        private void OkAction()
        {
            ParentWindow.DialogResult = true;
            ParentWindow.Close();
        }
    }
}
