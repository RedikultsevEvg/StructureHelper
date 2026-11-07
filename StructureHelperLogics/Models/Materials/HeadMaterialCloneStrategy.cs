using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperLogics.Models.Materials
{
    /// <summary>
    /// Item clone strategy for head materials used by collection cloning.
    /// </summary>
    public class HeadMaterialCloneStrategy : ICloneStrategy<IHeadMaterial>
    {
        public IHeadMaterial GetClone(IHeadMaterial sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference)
            {
                return sourceObject;
            }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);

            var clone = (IHeadMaterial)LegacyCloneHelper.Clone(sourceObject, cloningMode);
            CloneIdSynchronizer.Synchronize(sourceObject, clone, cloningMode);
            return clone;
        }
    }
}
