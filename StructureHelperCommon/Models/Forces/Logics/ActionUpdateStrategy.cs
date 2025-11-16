using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces
{
    public class ActionUpdateStrategy : IUpdateStrategy<IAction>
    {
        private IUpdateStrategy<IForceAction> forceUpdateStrategy;
        public void Update(IAction targetObject, IAction sourceObject)
        {
            forceUpdateStrategy ??= new ForceActionUpdateStrategy();
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            if (targetObject is IForceAction forceAction)
            {
                forceUpdateStrategy.Update(forceAction, (IForceAction)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(IAction), sourceObject.GetType());
            }
        }
    }
}
