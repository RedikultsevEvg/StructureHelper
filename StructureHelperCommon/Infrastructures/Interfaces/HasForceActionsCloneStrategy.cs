using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Collection clone strategy for objects which have force actions.
    /// </summary>
    public class HasForceActionsCloneStrategy : CollectionDeepCloningStrategy<IForceAction>
    {
        public HasForceActionsCloneStrategy(
            ICloningStrategy cloningStrategy,
            ICloneStrategy<IForceAction> forceActionCloneStrategy)
            : base(cloningStrategy, forceActionCloneStrategy)
        {
        }

        public HasForceActionsCloneStrategy(ICloningStrategy cloningStrategy)
            : this(cloningStrategy, new ForceActionCloneStrategy())
        {
        }

        public void GetClone(
            IHasForceActions target,
            IHasForceActions source,
            CloningMode mode = CloningMode.NewEntity)
        {
            CheckObject.ThrowIfNull(target);
            CheckObject.ThrowIfNull(source);
            GetClone(target.ForceActions, source.ForceActions, mode);
        }
    }
}
