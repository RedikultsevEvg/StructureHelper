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
        private IUpdateStrategy<IMaterialSafetyFactor> baseUpdateStrategy = new MaterialSafetyFactorBaseUpdateStrategy();
        public void Update(IMaterialSafetyFactor targetObject, IMaterialSafetyFactor sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.PartialFactors.Clear();
            foreach (var item in sourceObject.PartialFactors)
            {
                targetObject.PartialFactors.Add(item.Clone() as IMaterialPartialFactor);
            }
        }
    }
}
