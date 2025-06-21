using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class InclinedSectionUpdateStrategy : IUpdateStrategy<IInclinedSection>
    {
        public void Update(IInclinedSection targetObject, IInclinedSection sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            targetObject.StartCoord = sourceObject.StartCoord;
            targetObject.EndCoord = sourceObject.EndCoord;
            targetObject.EffectiveDepth = sourceObject.EffectiveDepth;
            targetObject.FullDepth = sourceObject.FullDepth;
            targetObject.LimitState = sourceObject.LimitState;
            targetObject.CalcTerm = sourceObject.CalcTerm;
            targetObject.ConcreteCompressionStrength = sourceObject.ConcreteCompressionStrength;
            targetObject.ConcreteTensionStrength = sourceObject.ConcreteTensionStrength;
            targetObject.WebWidth = sourceObject.WebWidth;
            targetObject.BeamShearSection = sourceObject.BeamShearSection.Clone() as IBeamShearSection;
        }
    }
}
