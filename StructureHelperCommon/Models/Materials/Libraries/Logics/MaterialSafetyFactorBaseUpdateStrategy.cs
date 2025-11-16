using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialSafetyFactorBaseUpdateStrategy : IUpdateStrategy<IMaterialSafetyFactor>
    {
        public void Update(IMaterialSafetyFactor targetObject, IMaterialSafetyFactor sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.Take = sourceObject.Take;
            targetObject.Description = sourceObject.Description;
        }
    }
}
