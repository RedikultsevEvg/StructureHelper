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
                    addCommand = new RelayCommand(o =>
                    {
                        AddMethod(o);
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
                    deleteCommand = new RelayCommand(o =>
                    {
                        DeleteMethod();
                    }, o => SelectedItem != null
                    ));
            }
        }
        public virtual void DeleteMethod()
        {
            Collection.Remove(SelectedItem);
            Items.Remove(SelectedItem);
        }
        public RelayCommand Edit
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(o=>
                    {
                        EditMethod(o);
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
                    (copyCommand = new RelayCommand (o=>
                    {
                        CopyMethod();
                    }, o => SelectedItem != null
                    ));
            }
        }
        public virtual void CopyMethod()
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
