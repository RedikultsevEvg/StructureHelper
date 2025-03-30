using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class BeamSectionShearStrengthLogic : IBeamShearStrenghLogic
    {
        private readonly ISectionEffectiveness sectionEffectiveness;
        private readonly double concreteStrength;
        private readonly IInclinedSection inclinedSection;

        private double crackLength;

        public BeamSectionShearStrengthLogic(
            ISectionEffectiveness sectionEffectiveness,
            double concreteStrength,
            IInclinedSection inclinedSection,
            IShiftTraceLogger? traceLogger)
        {
            this.sectionEffectiveness = sectionEffectiveness;
            this.concreteStrength = concreteStrength;
            this.inclinedSection = inclinedSection;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetShearStrength()
        {
            TraceLogger?.AddMessage($"Base shape factor = {sectionEffectiveness.BaseShapeFactor}, (dimensionless)");
            TraceLogger?.AddMessage($"Section shape factor = {sectionEffectiveness.ShapeFactor}, (dimensionless)");
            TraceLogger?.AddMessage($"Effective depth = {inclinedSection.EffectiveDepth}, (m)");
            crackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            TraceLogger?.AddMessage($"Crack length = {inclinedSection.EndCoord} - {inclinedSection.StartCoord} = {crackLength}, (m)");
            RestrictCrackLength();
            double concreteMoment = sectionEffectiveness.BaseShapeFactor * sectionEffectiveness.ShapeFactor * concreteStrength * inclinedSection.WebWidth * inclinedSection.EffectiveDepth * inclinedSection.EffectiveDepth;
            double shearStrength = concreteMoment / crackLength;
            TraceLogger?.AddMessage($"Shear strength of concrete = {shearStrength}, (N)");
            return shearStrength;
        }

        private void RestrictCrackLength()
        {
            double maxCrackLength = sectionEffectiveness.MaxCrackLengthRatio * inclinedSection.EffectiveDepth;
            if (crackLength > maxCrackLength)
            {
                TraceLogger?.AddMessage($"Crack length c = {crackLength} is greater than maximum crack length = {maxCrackLength}");
                crackLength = maxCrackLength;
                TraceLogger?.AddMessage($"Crack length = {crackLength}");
                return;
            }
            double minCrackLength = sectionEffectiveness.MinCrackLengthRatio * inclinedSection.EffectiveDepth;
            if (crackLength > minCrackLength)
            {
                TraceLogger?.AddMessage($"Crack length c = {crackLength} is less than minimum crack length = {minCrackLength}");
                crackLength = minCrackLength;
                TraceLogger?.AddMessage($"Crack length = {crackLength}");
            }
            return;
        }
    }
}
