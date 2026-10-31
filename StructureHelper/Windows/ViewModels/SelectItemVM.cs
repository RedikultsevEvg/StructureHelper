using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels
{
    /// <summary>
    /// View model which provides CRUD-operations and ordering for a collection of items.
    /// </summary>
    /// <typeparam name="TItem">Type of items</typeparam>
    public class SelectItemVM<TItem> : ViewModelBase, ICRUDViewModel<TItem> where TItem : class
    {
        private RelayCommand? addCommand;
        private RelayCommand? deleteCommand;
        private RelayCommand? copyCommand;
        private RelayCommand? editCommand;
        private RelayCommand? moveUpCommand;
        private RelayCommand? moveDownCommand;
        private RelayCommand? moveToFirstCommand;
        private RelayCommand? moveToEndCommand;

        /// <summary>
        /// Shadow (true) collection of items. It is kept in the same order as <see cref="Items"/>.
        /// </summary>
        public List<TItem> Collection { get; }

        /// <summary>
        /// Item which has been added recently.
        /// </summary>
        public TItem NewItem { get; set; }

        /// <summary>
        /// Item which is selected in the visual collection.
        /// </summary>
        public TItem SelectedItem
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(SelectedItem));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        /// <summary>
        /// Visual collection of items.
        /// </summary>
        public ObservableCollection<TItem> Items { get; private set; }

        public ICommand Add => addCommand ??= new RelayCommand(AddMethod);

        /// <summary>
        /// Encapsulates logic of adding an object when the Add-command runs.
        /// </summary>
        public virtual void AddMethod(object parameter)
        {
            if (NewItem is null)
            {
                return;
            }
            AddItem(NewItem);
        }

        public ICommand Delete => deleteCommand ??= new RelayCommand(DeleteMethod, o => SelectedItem is not null);

        /// <summary>
        /// Encapsulates logic of deleting of object when the Delete-command runs.
        /// </summary>
        public virtual void DeleteMethod(object parameter)
        {
            if (SelectedItem is null)
            {
                return;
            }
            RemoveItem(SelectedItem);
        }

        public ICommand Edit => editCommand ??= new RelayCommand(EditMethod, o => SelectedItem is not null);

        /// <summary>
        /// Encapsulates logic of editing of object when the Edit-command runs.
        /// </summary>
        public virtual void EditMethod(object parameter)
        {
            ReloadItems();
            AfterItemsEdit?.Invoke(this, new CRUDVMEventArgs());
        }

        public ICommand Copy => copyCommand ??= new RelayCommand(CopyMethod, o => SelectedItem is not null);

        /// <summary>
        /// Encapsulates logic of copying of object when the Copy-command runs.
        /// </summary>
        public virtual void CopyMethod(object parameter)
        {
            if (SelectedItem is not ICloneable cloneable)
            {
                return;
            }
            TItem? newItem = cloneable.Clone() as TItem;
            if (newItem is null)
            {
                return;
            }
            if (SelectedItem is ILogic sourceLogic
                && newItem is ILogic targetLogic
                && sourceLogic.TraceLogger is not null)
            {
                targetLogic.TraceLogger = sourceLogic.TraceLogger.GetSimilarTraceLogger();
            }
            NewItem = newItem;
            AddItem(newItem);
        }

        public ICommand MoveUpCommand => moveUpCommand ??= new RelayCommand(o => MoveUp(), o => CanMoveUp());

        public ICommand MoveDownCommand => moveDownCommand ??= new RelayCommand(o => MoveDown(), o => CanMoveDown());

        public ICommand MoveToFirstCommand => moveToFirstCommand ??= new RelayCommand(o => MoveToFirst(), o => CanMoveToFirst());

        public ICommand MoveToEndCommand => moveToEndCommand ??= new RelayCommand(o => MoveToEnd(), o => CanMoveToEnd());

        /// <summary>
        /// Moves the selected item one position up.
        /// </summary>
        /// <returns><c>true</c> if the item has been moved; otherwise <c>false</c>.</returns>
        public virtual bool MoveUp() => MoveSelectedItemBy(-1);

        /// <summary>
        /// Moves the selected item one position down.
        /// </summary>
        /// <returns><c>true</c> if the item has been moved; otherwise <c>false</c>.</returns>
        public virtual bool MoveDown() => MoveSelectedItemBy(1);

        /// <summary>
        /// Moves the selected item to the first position.
        /// </summary>
        /// <returns><c>true</c> if the item has been moved; otherwise <c>false</c>.</returns>
        public virtual bool MoveToFirst() => MoveSelectedItemTo(0);

        /// <summary>
        /// Moves the selected item to the last position.
        /// </summary>
        /// <returns><c>true</c> if the item has been moved; otherwise <c>false</c>.</returns>
        public virtual bool MoveToEnd() => MoveSelectedItemTo(Items.Count - 1);

        public bool CanMoveUp() => TryGetSelectedIndex(out int index) && index > 0;

        public bool CanMoveDown() => TryGetSelectedIndex(out int index) && index < Items.Count - 1;

        public bool CanMoveToFirst() => CanMoveUp();

        public bool CanMoveToEnd() => CanMoveDown();

        /// <summary>
        /// Adds items to the visual collection only. Use it when the backing <see cref="Collection"/>
        /// has already been updated by the caller.
        /// </summary>
        public void AddItems(IEnumerable<TItem> items)
        {
            foreach (TItem item in items)
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
            if (Items.Count > 0)
            {
                SelectedItem = Items[^1];
            }
            AfterItemsEdit?.Invoke(this, new CRUDVMEventArgs());
        }

        public delegate void CRUDHandler(SelectItemVM<TItem> sender, CRUDVMEventArgs e);
        public event CRUDHandler? AfterItemsEdit;

        private void AddItem(TItem item)
        {
            Collection.Add(item);
            Items.Add(item);
        }

        private void RemoveItem(TItem item)
        {
            Collection.Remove(item);
            Items.Remove(item);
        }

        private void ReloadItems()
        {
            Items.Clear();
            foreach (TItem item in Collection)
            {
                Items.Add(item);
            }
            OnPropertyChanged(nameof(Items));
        }

        private bool MoveSelectedItemBy(int offset)
        {
            if (TryGetSelectedIndex(out int index) == false)
            {
                return false;
            }
            return MoveItem(index, index + offset);
        }

        private bool MoveSelectedItemTo(int newIndex)
        {
            if (TryGetSelectedIndex(out int index) == false)
            {
                return false;
            }
            return MoveItem(index, newIndex);
        }

        /// <summary>
        /// Moves the item inside the visual collection and mirrors the new position in the backing collection.
        /// </summary>
        private bool MoveItem(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex
                || oldIndex < 0 || oldIndex >= Items.Count
                || newIndex < 0 || newIndex >= Items.Count)
            {
                return false;
            }

            TItem item = Items[oldIndex];
            Items.Move(oldIndex, newIndex);

            int collectionIndex = Collection.IndexOf(item);
            if (collectionIndex >= 0)
            {
                Collection.RemoveAt(collectionIndex);
                Collection.Insert(Math.Min(newIndex, Collection.Count), item);
            }
            return true;
        }

        private bool TryGetSelectedIndex(out int index)
        {
            if (SelectedItem is null)
            {
                index = -1;
                return false;
            }
            index = Items.IndexOf(SelectedItem);
            return index >= 0;
        }
    }
}
