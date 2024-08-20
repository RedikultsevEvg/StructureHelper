using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class LibMaterialUpdateStrategy : IUpdateStrategy<ILibMaterial>
    {
        public void Update(ILibMaterial targetObject, ILibMaterial sourceObject)
        {
            CheckObject.CompareTypes(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.MaterialEntity = sourceObject.MaterialEntity;
            if (targetObject.SafetyFactors is not null & sourceObject.SafetyFactors is not null)
            {
                targetObject.SafetyFactors.Clear();
                foreach (var item in sourceObject.SafetyFactors)
                {
                    targetObject.SafetyFactors.Add(item.Clone() as IMaterialSafetyFactor);
                }
            }
            targetObject.MaterialLogic = sourceObject.MaterialLogic;

        }
    }
}
