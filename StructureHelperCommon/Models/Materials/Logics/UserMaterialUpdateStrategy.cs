using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Materials
{
    public class UserMaterialUpdateStrategy : IParentUpdateStrategy<IUserMaterial>
    {
        private IUpdateStrategy<IHelperMaterial> safetyFactorsUpdateStrategy;
        private IUpdateStrategy<IHelperMaterial> SafetyFactorsUpdateStrategy => safetyFactorsUpdateStrategy ??= new HelpermaterialSafetyFactorsUpdateStrategy();
        public bool UpdateChildren { get; set; } = true;

        public void Update(IUserMaterial targetObject, IUserMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.FilePath = sourceObject.FilePath;
            if (UpdateChildren == true)
            {
                SafetyFactorsUpdateStrategy.Update(targetObject, sourceObject);
            }
        }
    }
}
