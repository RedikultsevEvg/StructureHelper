using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Repositories
{
    public class ListRepository<T> : IDataRepository<T>
        where T : ISaveable
    {
        List<T> internalData;
        private IUpdateStrategy<T> updateStrategy;
        public void Create(T entity)
        {
            var isExists = internalData.Exists(x => x.Id == entity.Id);
            if (isExists == true)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $":\nobject with Id={entity.Id} already exists");
            }
            internalData.Add(entity);
        }

        public void Delete(Guid Id)
        {
            T existingObject = GetById(Id);
            internalData.Remove(existingObject);
        }

        public List<T> GetAll()
        {
            return new List<T>(internalData);
        }

        public T GetById(Guid id)
        {
            T existingObject = internalData.Single(x => x.Id == id);
            return existingObject;
        }

        public void Update(T updateObject)
        {
            T existingObject = GetById(updateObject.Id);
            updateStrategy.Update(existingObject, updateObject);
        }
        public ListRepository(List<T> initialData, IUpdateStrategy<T> updateStrategy)
        {
            internalData = initialData;
            this.updateStrategy = updateStrategy;
        }
        public ListRepository(IUpdateStrategy<T> updateStrategy) : this(new List<T>(), updateStrategy)
        {
        }
    }
}
