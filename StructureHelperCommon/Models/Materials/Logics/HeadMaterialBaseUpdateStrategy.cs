using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Materials
{
    public class HeadMaterialBaseUpdateStrategy : IUpdateStrategy<IHeadMaterial>
    {
        public void Update(IHeadMaterial targetObject, IHeadMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.Color = sourceObject.Color;
        }
    }
}
