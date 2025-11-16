using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class UniformlyDisributedLoadUpdateStrategy : IUpdateStrategy<IDistributedLoad>
    {
        public void Update(IDistributedLoad targetObject, IDistributedLoad sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.LoadValue = sourceObject.LoadValue;
            targetObject.RelativeLoadLevel = sourceObject.RelativeLoadLevel;
        }
    }
}
