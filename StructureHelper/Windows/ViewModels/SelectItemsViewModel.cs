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
    public class SelectItemsViewModel<TItem> : ViewModelBase
        where TItem : class
    {
        private ICommand? selectAllCommand;
        private ICommand? unSelectAllCommand;
        private ICommand? invertSelectionCommand;
        private IEnumerable<TItem> selectedItems;

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
        public bool ShowButtons { get; set; }
        public ObservableCollection<CollectionItem> CollectionItems { get; }

        public ICommand SelectAllCommand
        {
            get
            {
                return selectAllCommand ??= new RelayCommand(o => SetIsSelected(true));
            }
        }

        public ICommand UnSelectAllCommand => unSelectAllCommand ??= new RelayCommand(o => SetIsSelected(false));

        public ICommand InvertSelectionCommand
        {
            get
            {
                return invertSelectionCommand ??= new RelayCommand(o => InvertSelection());
            }
        }

        private void InvertSelection()
        {
            {
                foreach (var item in CollectionItems)
                {
                    item.IsSelected = !item.IsSelected;
                }
            };
        }

        private void SetIsSelected(bool isSelected)
        {
            foreach (var item in CollectionItems)
            {
                item.IsSelected = isSelected;
            }
        }

        public SelectItemsViewModel(IEnumerable<TItem> items)
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
