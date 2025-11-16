using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSectionLogicInputDataUpdateStrategy : IUpdateStrategy<IBeamShearSectionLogicInputData>
    {
        public void Update(IBeamShearSectionLogicInputData targetObject, IBeamShearSectionLogicInputData sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.BeamShearAction = sourceObject.BeamShearAction;
            targetObject.InclinedSection = sourceObject.InclinedSection;
            targetObject.InclinedCrack = sourceObject.InclinedCrack;
            targetObject.Stirrup = sourceObject.Stirrup;
            targetObject.LimitState = sourceObject.LimitState;
            targetObject.CalcTerm = sourceObject.CalcTerm;
            targetObject.ForceTuple = sourceObject.ForceTuple;
        }
    }
}
