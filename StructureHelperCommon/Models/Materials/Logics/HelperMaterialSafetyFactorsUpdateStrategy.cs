using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Materials
{
    public class HelpermaterialSafetyFactorsUpdateStrategy : IUpdateStrategy<IHelperMaterial>
    {
        public void Update(IHelperMaterial targetObject, IHelperMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
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
