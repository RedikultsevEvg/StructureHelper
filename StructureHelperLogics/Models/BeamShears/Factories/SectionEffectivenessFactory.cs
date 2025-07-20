using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public static class SectionEffectivenessFactory
    {
        public static ISectionEffectiveness GetShearEffectiveness(BeamShearSectionType sectionType)
        {
            if (sectionType == BeamShearSectionType.Rectangle)
            {
                return GetRectangleEffectiveness();
            }
            else if (sectionType == BeamShearSectionType.Circle)
            {
                return GetCircleEffectiveness();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sectionType));
            }
        }

        private static ISectionEffectiveness GetCircleEffectiveness()
        {
            SectionEffectiveness sectionEffectiveness = new()
            {
                BaseShapeFactor = 1.5,
                MaxCrackLengthRatio = 3,
                MinCrackLengthRatio = 0.6,
                ShapeFactor = 0.6
            };
            return sectionEffectiveness;
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
