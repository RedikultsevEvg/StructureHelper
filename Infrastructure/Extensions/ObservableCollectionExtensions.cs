using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StructureHelper.Infrastructure.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void MoveElementToEnd<T>(this ObservableCollection<T> collection, T element)
        {
            var elementCopy = element;
            collection.Remove(element);
            collection.Add(elementCopy);
        }

        public static void MoveElementToBegin<T>(this ObservableCollection<T> collection, T element)
        {
            var collectionCopy = new List<T>(collection);
            collectionCopy.Remove(element);
            collection.Clear();
            collection.Add(element);
            foreach (var collectionElem in collectionCopy)
                collection.Add(collectionElem);
        }

        public static void MoveElementToSelectedIndex<T>(this ObservableCollection<T> collection, T element, int index)
        {
            collection.Remove(element);
            collection.Insert(index - 1, element);
        }
    }
}
