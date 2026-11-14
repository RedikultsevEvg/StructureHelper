using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Item clone strategy for force actions used by collection cloning.
    /// </summary>
    public class ForceActionCloneStrategy : ICloneStrategy<IForceAction>
    {
        private IUpdateStrategy<IForceAction> updateStrategy;
        private IUpdateStrategy<IForceAction> UpdateStrategy => updateStrategy ??= new ForceActionBaseUpdateStrategy();

        public IForceAction GetClone(IForceAction sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference)
            {
                return sourceObject;
            }
            IForceAction clone = GetChildClone(sourceObject, cloneMode, cloneDepth);
            UpdateStrategy.Update(clone, sourceObject);
            if (cloneMode == CloneMode.PreserveIdentity)
            {
                CloneFallback.SynchronizeIds(sourceObject, clone);
            }
            return clone;
        }

        private IForceAction GetChildClone(IForceAction sourceObject, CloneMode cloningMode, CloneDepth cloneDepth)
        {
            if (sourceObject is IDesignForcePair pair)
            {
                return pair.GetClone(cloningMode, cloneDepth);
            }
            else if (sourceObject is IForceFactoredList combination)
            {
                return combination.GetClone(cloningMode, cloneDepth);
            }
            else if (sourceObject is IForceCombinationList forceCombinationList)
            {
                return forceCombinationList.GetClone(cloningMode, cloneDepth);
            }
            else if (sourceObject is IForceCombinationFromFile fileCombination)
            {
                return fileCombination.GetClone(cloningMode, cloneDepth);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject));
            }
        }
    }
}
