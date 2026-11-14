using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public class CollectionDeepCloningStrategy<T> : ICollectionCloneStrategy<T> where T : class
    {
        private ICloningStrategy cloningStrategy;
        private ICloneStrategy<T> cloneStrategy;

        public CollectionDeepCloningStrategy(ICloningStrategy cloningStrategy, ICloneStrategy<T> cloneStrategy)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(cloneStrategy);
            this.cloningStrategy = cloningStrategy;
            this.cloneStrategy = cloneStrategy;
        }

        public void GetClone(ICollection<T> target, IEnumerable<T> source, CloneMode mode = CloneMode.NewEntity)
        {
            CheckObject.ThrowIfNull(target);
            CheckObject.ThrowIfNull(source);
            var clones = source
                .Select(item => cloningStrategy.Clone(item, cloneStrategy, mode))
                .ToList();

            target.Clear();

            foreach (var clone in clones)
            {
                target.Add(clone);
            }
        }
    }
}
