using StructureHelperCommon.Infrastructures.Enums;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    internal class CollectionCloneStrategy<T> : ICollectionCloneStrategy<T> where T : class
    {
        private readonly ICloneStrategy<T> _itemCloner;

        public CollectionCloneStrategy(
            ICloneStrategy<T> itemCloner)
        {
            _itemCloner = itemCloner;
        }

        public void GetClone(
            ICollection<T> target,
            IEnumerable<T> source,
            CloneMode mode = CloneMode.NewEntity)
        {
            var clones = source
                .Select(item => _itemCloner.GetClone(item, mode))
                .ToList();

            target.Clear();

            foreach (var clone in clones)
            {
                target.Add(clone);
            }
        }
    }
}
