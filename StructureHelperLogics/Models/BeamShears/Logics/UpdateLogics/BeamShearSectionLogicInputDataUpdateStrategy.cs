using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSectionLogicInputDataUpdateStrategy : IUpdateStrategy<IBeamShearSectionLogicInputData>
    {
        public void Update(IBeamShearSectionLogicInputData targetObject, IBeamShearSectionLogicInputData sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.InclinedSection = sourceObject.InclinedSection;
            targetObject.Stirrup = sourceObject.Stirrup;
            targetObject.LimitState = sourceObject.LimitState;
            targetObject.CalcTerm = sourceObject.CalcTerm;
            targetObject.ForceTuple = sourceObject.ForceTuple;
        }
    }
}
