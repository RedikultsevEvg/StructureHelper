using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperLogics.Models.BeamShears
{
    public class SectionEffectivenessFactorySP2018 : ISectionEffectivenessFactory
    {
        public ISectionEffectiveness GetShearEffectiveness(BeamShearSectionType sectionType)
        {
            if (sectionType == BeamShearSectionType.Rectangle)
            {
                return GetRectangleEffectiveness();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sectionType));
            }
        }

        private static ISectionEffectiveness GetRectangleEffectiveness()
        {
            SectionEffectiveness sectionEffectiveness = new()
            {
                BaseShapeFactor = 1.5,
                MaxCrackLengthRatio = 3,
                MinCrackLengthRatio = 0.6,
                ShapeFactor = 1
            };
            return sectionEffectiveness;
        }
    }
}
