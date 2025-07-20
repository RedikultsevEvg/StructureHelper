using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public enum BeamShearSectionType
    {
        Rectangle,
        Circle
    }
    public static class StirrupEffectivenessFactory
    {
        public static IStirrupEffectiveness GetEffectiveness(BeamShearSectionType sectionType)
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

        private static IStirrupEffectiveness GetCircleEffectiveness()
        {
            StirrupEffectiveness stirrupEffectiveness = new()
            {
                MaxCrackLengthRatio = 1.5,
                StirrupPlacementFactor = 0.75,
                StirrupShapeFactor = 0.5,
                MinimumStirrupRatio = 0.1
            };
            return stirrupEffectiveness;
        }

        private static IStirrupEffectiveness GetRectangleEffectiveness()
        {
            StirrupEffectiveness stirrupEffectiveness = new()
            {
                MaxCrackLengthRatio = 2,
                StirrupPlacementFactor = 0.75,
                StirrupShapeFactor = 1,
                MinimumStirrupRatio = 0.25
            };
            return stirrupEffectiveness;
        }
    }
}
