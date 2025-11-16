using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Add objects from one repository to another one without deleting previous objects
    /// </summary>
    public class BeamShearRepositoryAddUpdateStrategy : IUpdateStrategy<IBeamShearRepository>
    {
        public void Update(IBeamShearRepository targetObject, IBeamShearRepository sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            targetObject.Actions.AddRange(sourceObject.Actions);
            targetObject.Sections.AddRange(sourceObject.Sections);
            targetObject.Stirrups.AddRange(sourceObject.Stirrups);
            targetObject.Calculators.AddRange(sourceObject.Calculators);
        }
    }
}
