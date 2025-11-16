using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearDesignRangePropertyUpdateStrategy : IUpdateStrategy<IBeamShearDesignRangeProperty>
    {
        public void Update(IBeamShearDesignRangeProperty targetObject, IBeamShearDesignRangeProperty sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.AbsoluteRangeValue = sourceObject.AbsoluteRangeValue;
            targetObject.RelativeEffectiveDepthRangeValue = sourceObject.RelativeEffectiveDepthRangeValue;
            targetObject.StepCount = sourceObject.StepCount;
        }
    }
}
