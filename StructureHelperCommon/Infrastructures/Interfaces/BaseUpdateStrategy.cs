using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public abstract class BaseUpdateStrategy<T> : IUpdateStrategy<T>
    {
        public abstract void Update(T targetObject, T sourceObject);
        public void Check(T targetObject, T sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
        }
    }
}
