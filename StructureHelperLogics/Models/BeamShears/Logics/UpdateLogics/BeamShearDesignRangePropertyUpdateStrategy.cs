using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearDesignRangePropertyUpdateStrategy : IUpdateStrategy<IBeamShearDesignRangeProperty>
    {
        public void Update(IBeamShearDesignRangeProperty targetObject, IBeamShearDesignRangeProperty sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.AbsoluteRangeValue = sourceObject.AbsoluteRangeValue;
            targetObject.RelativeEffectiveDepthRangeValue = sourceObject.RelativeEffectiveDepthRangeValue;
            targetObject.StepCount = sourceObject.StepCount;
            targetObject.RelativeEffectiveDepthSectionLengthMaxValue = sourceObject.RelativeEffectiveDepthSectionLengthMaxValue;
            targetObject.RelativeEffectiveDepthSectionLengthMinValue = sourceObject.RelativeEffectiveDepthSectionLengthMinValue;
        }
    }
}
