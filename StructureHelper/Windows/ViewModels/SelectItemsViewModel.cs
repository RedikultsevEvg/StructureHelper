using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels
{
    public class SelectItemsViewModel<TItem> : ViewModelBase
        where TItem : class
    {
        private ICommand? selectAllCommand;
        private ICommand? unSelectAllCommand;
        private ICommand? invertSelectionCommand;

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

        public DataTemplate ItemDataDemplate { get; set; }
        public bool ShowButtons { get; set; }
        public ObservableCollection<CollectionItem> CollectionItems { get; }

        public ICommand SelectAllCommand
        {
            get
            {
                return selectAllCommand ??= new RelayCommand(o => setIsSelected(true));
            }
        }

        public ICommand UnSelectAllCommand
        {
            get
            {
                return unSelectAllCommand ??= new RelayCommand(o => setIsSelected(false));
            }
        }

        public ICommand InvertSelectionCommand
        {
            get
            {
                return invertSelectionCommand ??= new RelayCommand(o =>
                    {
                        foreach (var item in CollectionItems)
                        {
                            item.IsSelected = !item.IsSelected;
                        }
                    }
                    );
            }
        }

        private void setIsSelected(bool isSelected)
        {
            foreach (var item in CollectionItems)
            {
                item.IsSelected = isSelected;
            }
        }

        public SelectItemsViewModel(IEnumerable<TItem> items)
        {
            CollectionItems = new ObservableCollection<CollectionItem>();
            foreach (var item in items)
            {
                CollectionItems.Add(new CollectionItem() { IsSelected = true, Item = item });
            }
        }
    }
}
