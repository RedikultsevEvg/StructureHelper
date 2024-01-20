using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.ViewModels
{
    /// <summary>
    /// Represents a ViewModel for selecting items from a collection.
    /// </summary>
    /// <typeparam name="TItem">The type of items in the collection.</typeparam>
    public class SelectItemsVM<TItem> : ViewModelBase
        where TItem : class
    {
        private ICommand? selectAllCommand;
        private ICommand? unSelectAllCommand;
        private ICommand? invertSelectionCommand;
        private IEnumerable<TItem> selectedItems;
        /// <summary>
        /// Class for item of collection
        /// </summary>
        public class CollectionItem : ViewModelBase
        {
            bool isSelected;
            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
            public TItem Item { get; set; }
        }

        public DataTemplate ItemDataTemplate { get; set; }
        /// <summary>
        /// Turns buttons on or off
        /// </summary>
        public bool ShowButtons { get; set; }
        /// <summary>
        /// Collections of items
        /// </summary>
        public ObservableCollection<CollectionItem> CollectionItems { get; }
        /// <summary>
        /// Selects all items
        /// </summary>
        public ICommand SelectAllCommand
        {
            get
            {
                return selectAllCommand ??= new RelayCommand(o => SetIsSelected(true));
            }
        }
        /// <summary>
        /// Deselects all items
        /// </summary>
        public ICommand UnSelectAllCommand => unSelectAllCommand ??= new RelayCommand(o => SetIsSelected(false));
        /// <summary>
        /// Reverts selection of item from on to off
        /// </summary>
        public ICommand InvertSelectionCommand
        {
            get
            {
                return invertSelectionCommand ??= new RelayCommand(o => InvertSelection());
            }
        }

        public void InvertSelection()
        {
            {
                foreach (var item in CollectionItems)
                {
                    item.IsSelected = !item.IsSelected;
                }
            };
        }
        /// <summary>
        /// Select all if true, deselect all if false
        /// </summary>
        /// <param name="isSelected">Default is true</param>
        public void SetIsSelected(bool isSelected = true)
        {
            foreach (var item in CollectionItems)
            {
                item.IsSelected = isSelected;
            }
        }

        public SelectItemsVM(IEnumerable<TItem> items)
        {
            CollectionItems = new ObservableCollection<CollectionItem>(
                items
                .Select(item => new CollectionItem
                {
                    IsSelected = true,
                    Item = item
                })
            );
        }
        /// <summary>
        /// Gets or sets collection of items which are selected
        /// </summary>
        public IEnumerable<TItem> SelectedItems
        {
            get => CollectionItems.Where(x => x.IsSelected == true).Select(x => x.Item);
            set
            {
                // Check if all provided items are contained in the main collection
                if (value.All(item => CollectionItems.Any(ci => ci.Item.Equals(item))))
                {
                    selectedItems = value;

                    // Update the IsSelected property based on the provided selected items
                    foreach (var item in CollectionItems)
                    {
                        item.IsSelected = selectedItems.Contains(item.Item);
                    }

                    OnPropertyChanged(nameof(SelectedItems));
                }
                else
                {
                    // Handle the case where not all items are in the main collection
                    // You might throw an exception or log a message
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": Not all items are contained in the main collection");
                }
            }
        }
        public int SelectedCount => SelectedItems.Count();
    }
}
