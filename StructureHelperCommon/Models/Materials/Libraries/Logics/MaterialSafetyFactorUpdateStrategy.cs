using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    internal class MaterialSafetyFactorUpdateStrategy : IUpdateStrategy<IMaterialSafetyFactor>
    {
        public void Update(IMaterialSafetyFactor targetObject, IMaterialSafetyFactor sourceObject)
        {
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
