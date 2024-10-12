using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials.Logics
{
    public class HelpermaterialSafetyFactorsUpdateStrategy : IUpdateStrategy<IHelperMaterial>
    {
        public void Update(IHelperMaterial targetObject, IHelperMaterial sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (sourceObject.SafetyFactors is not null)
            {
                if (targetObject.SafetyFactors is null)
                {
                    targetObject.SafetyFactors = new();
                }
                targetObject.SafetyFactors.Clear();
                foreach (var item in sourceObject.SafetyFactors)
                {
                    targetObject.SafetyFactors.Add(item.Clone() as IMaterialSafetyFactor);
                }
            }
        }
    }
}
