using StructureHelperCommon.Models;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class RestrictInclinedCrackSP63Logic : IGetCrackLengthLogic
    {
        public RestrictInclinedCrackSP63Logic(
            ISectionEffectiveness sectionEffectiveness,
            IInclinedSection inclinedSection,
            IShiftTraceLogger? traceLogger)
        {
            SectionEffectiveness = sectionEffectiveness;
            InclinedSection = inclinedSection;
            TraceLogger = traceLogger;
        }

        public ISectionEffectiveness SectionEffectiveness { get; }
        public IInclinedSection InclinedSection { get; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetCrackLength(double sourceLength)
        {
            double maxLength = SectionEffectiveness.MaxCrackLengthRatio * InclinedSection.EffectiveDepth;
            double minLength = SectionEffectiveness.MinCrackLengthRatio * InclinedSection.EffectiveDepth;

            if (sourceLength > maxLength)
            {
                TraceLogger?.AddMessage($"Max design crack length cmax = {SectionEffectiveness.MaxCrackLengthRatio} * {InclinedSection.EffectiveDepth}{UnitsOfSI.Meters} = {maxLength}{UnitsOfSI.Meters}. Crack length has been reduced from {sourceLength}{UnitsOfSI.Meters} to {maxLength}{UnitsOfSI.Meters}");
                return maxLength;
            }
            if (sourceLength < minLength)
            {
                TraceLogger?.AddMessage($"Min design crack length cmin = {SectionEffectiveness.MinCrackLengthRatio} * {InclinedSection.EffectiveDepth}{UnitsOfSI.Meters} = {minLength}{UnitsOfSI.Meters}. Crack length has been increased from {sourceLength}{UnitsOfSI.Meters} to {minLength}{UnitsOfSI.Meters}");
                return minLength;
            }
            return sourceLength;
        }
    }
}
