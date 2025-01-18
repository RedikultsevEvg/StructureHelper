using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Exceptions;
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
            Check();
            ParentWindow.DialogResult = false;
            ParentWindow.Close();
        }

        private void Check()
        {
            if (ParentWindow is null)
            {
                throw new StructureHelperException(ErrorStrings.NullReference + ": Parent window");
            }
        }

        public virtual void OkAction()
        {
            Check();
            ParentWindow.DialogResult = true;
            ParentWindow.Close();
        }
    }
}
