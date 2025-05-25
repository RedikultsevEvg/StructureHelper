using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasBeamShearActionUpdateStrategy : IUpdateStrategy<IHasBeamShearActions>
    {
        public void Update(IHasBeamShearActions targetObject, IHasBeamShearActions sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            CheckObject.IsNull(sourceObject.Actions);
            CheckObject.IsNull(targetObject.Actions);
            targetObject.Actions.Clear();
            targetObject.Actions.AddRange(sourceObject.Actions);
        }
    }
}
