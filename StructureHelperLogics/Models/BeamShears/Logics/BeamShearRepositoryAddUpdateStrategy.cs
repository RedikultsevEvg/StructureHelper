using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearRepositoryAddUpdateStrategy : IUpdateStrategy<IBeamShearRepository>
    {
        public void Update(IBeamShearRepository targetObject, IBeamShearRepository sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            targetObject.Actions.AddRange(sourceObject.Actions);
            targetObject.Sections.AddRange(sourceObject.Sections);
            targetObject.Stirrups.AddRange(sourceObject.Stirrups);
            targetObject.Calculators.AddRange(sourceObject.Calculators);
        }
    }
}
