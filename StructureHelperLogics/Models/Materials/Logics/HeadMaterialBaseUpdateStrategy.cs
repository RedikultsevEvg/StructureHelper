using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class HeadMaterialBaseUpdateStrategy : IUpdateStrategy<IHeadMaterial>
    {
        public void Update(IHeadMaterial targetObject, IHeadMaterial sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.Color = sourceObject.Color;
        }
    }
}
