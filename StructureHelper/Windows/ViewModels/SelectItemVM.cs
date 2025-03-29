using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels
{
    /// <summary>
    /// View model which provides CRUD-operations with collection of items 
    /// </summary>
    /// <typeparam name="TItem">Type of items</typeparam>
    public class SelectItemVM<TItem> : ViewModelBase, ICRUDViewModel<TItem> where TItem : class
    {
        private ICommand addCommand;
        private ICommand deleteCommand;
        private ICommand copyCommand;
        private ICommand editCommand;

        /// <summary>
        /// Shadow (true) collection of items
        /// </summary>
        public List<TItem> Collection { get; set; }
        /// <summary>
        /// Items which has been added recently
        /// </summary>
        public TItem NewItem { get; set; }
        /// <summary>
        /// Item which selected in visual collection
        /// </summary>
        public TItem SelectedItem { get; set; }
        /// <summary>
        /// Visual collection of items
        /// </summary>
        public ObservableCollection<TItem> Items { get; private set; }

        public ICommand Add
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
        public ICommand Delete
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
        public ICommand Edit
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
        /// <summary>
        /// Incaplsulates logic of editing of object when Edit-command runs
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void EditMethod(object parameter)
        {
            Items.Clear();
            foreach (var item in Collection)
            {
                Items.Add(item);
            }
            OnPropertyChanged(nameof(Items));
            AfterItemsEdit?.Invoke(this, new CRUDVMEventArgs());
        }

        public ICommand Copy
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
            if (SelectedItem is ILogic logic)
            {
                if (logic.TraceLogger is not null)
                {
                    (NewItem as ILogic).TraceLogger = logic.TraceLogger.GetSimilarTraceLogger();
                }
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

        public SelectItemVM(List<TItem> collection)
        {
            Collection = collection;
            Refresh();
        }
        public void Refresh()
        {
            Items = new ObservableCollection<TItem>(Collection);
            OnPropertyChanged(nameof(Items));
            if (Items.Any())
            {
                SelectedItem = Items[^1];
            }
            AfterItemsEdit?.Invoke(this, new CRUDVMEventArgs());
        }
        public delegate void CRUDHandler(SelectItemVM<TItem> sender, CRUDVMEventArgs e);
        public event CRUDHandler? AfterItemsEdit;
    }
}
