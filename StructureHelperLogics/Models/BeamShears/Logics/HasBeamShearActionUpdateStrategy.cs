using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasBeamShearActionUpdateStrategy : IUpdateStrategy<IHasBeamShearActions>
    {
        public void Update(IHasBeamShearActions targetObject, IHasBeamShearActions sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            CheckObject.ThrowIfNull(sourceObject.Actions);
            CheckObject.ThrowIfNull(targetObject.Actions);
            targetObject.Actions.Clear();
            targetObject.Actions.AddRange(sourceObject.Actions);
        }
    }
}
