using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IRepositoryOperation<T, V>
    {
        void RemoveAll();
        void Remove(V entity);
        void Remove(IEnumerable<V> entities);
    }
}
