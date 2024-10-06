using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialSafetyFactorUpdateStrategy : IUpdateStrategy<IMaterialSafetyFactor>
    {
        public void Update(IMaterialSafetyFactor targetObject, IMaterialSafetyFactor sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.Take = sourceObject.Take;
            targetObject.Description = sourceObject.Description;
            targetObject.PartialFactors.Clear();
            foreach (var item in sourceObject.PartialFactors)
            {
                targetObject.PartialFactors.Add(item.Clone() as IMaterialPartialFactor);
            }
        }
    }
}
