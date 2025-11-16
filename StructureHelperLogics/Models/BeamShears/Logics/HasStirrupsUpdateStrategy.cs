using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasStirrupsUpdateStrategy : IUpdateStrategy<IHasStirrups>
    {
        public void Update(IHasStirrups targetObject, IHasStirrups sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            CheckObject.ThrowIfNull(sourceObject.Stirrups);
            CheckObject.ThrowIfNull(targetObject.Stirrups);
            targetObject.Stirrups.Clear();
            targetObject.Stirrups.AddRange(sourceObject.Stirrups);
        }
    }
}
