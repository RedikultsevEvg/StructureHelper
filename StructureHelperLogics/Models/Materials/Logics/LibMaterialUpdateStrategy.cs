using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    internal class LibMaterialUpdateStrategy : IUpdateStrategy<ILibMaterial>
    {
        public void Update(ILibMaterial targetObject, ILibMaterial sourceObject)
        {
            targetObject.MaterialEntity = sourceObject.MaterialEntity;
            targetObject.SafetyFactors.Clear();
            foreach (var item in sourceObject.SafetyFactors)
            {
                targetObject.SafetyFactors.Add(item.Clone() as IMaterialSafetyFactor);
            }
        }
    }
}
