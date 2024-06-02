using StructureHelper.Infrastructure;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels
{
    public abstract class OkCancelViewModelBase : ViewModelBase
    {
        public Window ParentWindow { get; set; }
        public ICommand OkCommand => new RelayCommand(o => OkAction());
        public ICommand CancelCommand => new RelayCommand(o => CancelAction());
        public virtual void CancelAction()
        {
            ParentWindow.DialogResult = false;
            ParentWindow.Close();
        }

        public virtual void OkAction()
        {
            ParentWindow.DialogResult = true;
            ParentWindow.Close();
        }
    }
}
