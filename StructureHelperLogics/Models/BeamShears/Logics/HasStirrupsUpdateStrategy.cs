using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasStirrupsUpdateStrategy : IUpdateStrategy<IHasStirrups>
    {
        public void Update(IHasStirrups targetObject, IHasStirrups sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            CheckObject.IsNull(sourceObject.Stirrups);
            CheckObject.IsNull(targetObject.Stirrups);
            targetObject.Stirrups.Clear();
            targetObject.Stirrups.AddRange(sourceObject.Stirrups);
        }
    }
}
