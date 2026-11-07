using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Item clone strategy for force actions used by collection cloning.
    /// </summary>
    public class ForceActionCloneStrategy : ICloneStrategy<IForceAction>
    {
        public IForceAction GetClone(IForceAction sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference)
            {
                return sourceObject;
            }

            var clone = (IForceAction)LegacyCloneHelper.Clone(sourceObject, cloningMode);
            CloneIdSynchronizer.Synchronize(sourceObject, clone, cloningMode);
            return clone;
        }
    }
}
