using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ICollectionCloneStrategy<T> where T : class
    {
        void GetClone(
        ICollection<T> target,
        IEnumerable<T> source,
        CloningMode mode = CloningMode.NewEntity);
    }
}
