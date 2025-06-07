using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class HasForceCombinationUpdateStrategy : IUpdateStrategy<IHasForceActions>
    {
        public void Update(IHasForceActions targetObject, IHasForceActions sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject, "Interface IHasForceCombination");
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ForceActions.Clear();
            targetObject.ForceActions.AddRange(sourceObject.ForceActions);
        }
    }
}
