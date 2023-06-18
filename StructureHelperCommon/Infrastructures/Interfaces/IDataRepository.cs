using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IDataRepository<T>
    {
        void Save(T entity);
        T Load(Guid Id);

    }
}
