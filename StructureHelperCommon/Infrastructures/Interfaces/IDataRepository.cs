using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IDataRepository<T>
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(Guid Id);
        T GetById(Guid Id);
        List<T> GetAll();
    }
}
