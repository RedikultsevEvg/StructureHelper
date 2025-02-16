using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class UniformlyDisributedLoadUpdateStrategy : IUpdateStrategy<IUniformlyDistributedLoad>
    {
        public void Update(IUniformlyDistributedLoad targetObject, IUniformlyDistributedLoad sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.LoadValue = sourceObject.LoadValue;
            targetObject.RelativeLoadLevel = sourceObject.RelativeLoadLevel;
        }
    }
}
