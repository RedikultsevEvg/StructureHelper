using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace StructureHelper.Windows.ViewModels
{
    public abstract class CRUDViewModelBase<TItem> : ViewModelBase, ICRUDViewModel<TItem> where TItem : class
    {

        private RelayCommand addCommand;
        private RelayCommand deleteCommand;
        private RelayCommand copyCommand;
        private RelayCommand editCommand;

        public List<TItem> Collection { get; set; }

        public TItem NewItem { get; set; }
        public TItem SelectedItem { get; set; }

        public ObservableCollection<TItem> Items { get; private set; }

        public RelayCommand Add
        {
            get
            {
                return addCommand ??
                    (
                    addCommand = new RelayCommand(param =>
                    {
                        AddMethod(param);
                    }
                    ));
            }
        }
        public virtual void AddMethod(object parameter)
        {
            Collection.Add(NewItem);
            Items.Add(NewItem);
        }
        public RelayCommand Delete
        {
            get
            {
                return deleteCommand ??
                    (
                    deleteCommand = new RelayCommand(param =>
                    {
                        DeleteMethod(param);
                    }, o => SelectedItem != null
                    ));
            }
        }
        public virtual void DeleteMethod(object parameter)
        {
            Collection.Remove(SelectedItem);
            Items.Remove(SelectedItem);
        }
        public RelayCommand Edit
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(param=>
                    {
                        EditMethod(param);
                    }, o => SelectedItem != null
                    ));
            }
        }

        public virtual void EditMethod(object parameter)
        {
            Items.Clear();
            foreach (var item in Collection)
            {
                Items.Add(item);
            }
            OnPropertyChanged(nameof(Items));
        }

        public RelayCommand Copy
        {
            get
            {
                return copyCommand ??
                    (copyCommand = new RelayCommand (param=>
                    {
                        CopyMethod(param);
                    }, o => SelectedItem != null
                    ));
            }
        }
        public virtual void CopyMethod(object parameter)
        {
            if (SelectedItem is ICloneable)
            {
                NewItem = (SelectedItem as ICloneable).Clone() as TItem;
            }
            Collection.Add(NewItem);
            Items.Add(NewItem);
        }
        public void AddItems(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        public CRUDViewModelBase()
        {
            Items = new ObservableCollection<TItem>();
        }

        public CRUDViewModelBase(List<TItem> collection)
        {
            Collection = collection;
            Items = new ObservableCollection<TItem>(collection);
        }
    }
}
