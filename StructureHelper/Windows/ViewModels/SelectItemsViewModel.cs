using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StructureHelper.Windows.ViewModels
{
    public class SelectItemsViewModel<TItem>
        where TItem : class
    {
        public class CollectionItem
        {
            public bool IsSelected { get; set; }
            public TItem Item { get; set; }
        }

        public DataTemplate ItemDataDemplate { get; set; }

        public ObservableCollection<CollectionItem> CollectionItems { get; }

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
