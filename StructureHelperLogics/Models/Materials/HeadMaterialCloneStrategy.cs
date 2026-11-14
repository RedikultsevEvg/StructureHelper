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
        public IHeadMaterial GetClone(IHeadMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference)
            {
                return sourceObject;
            }
            var clone = sourceObject.GetClone(cloneMode, cloneDepth);
            return clone;
        }
    }
}
