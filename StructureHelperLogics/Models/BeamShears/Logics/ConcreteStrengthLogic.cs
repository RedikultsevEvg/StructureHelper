using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.BeamShears.Logics;

namespace StructureHelperLogics.Models.BeamShears
{
    public class ConcreteStrengthLogic : IBeamShearStrenghLogic
    {
        private readonly double longitudinalForce;
        private readonly ISectionEffectiveness sectionEffectiveness;
        private readonly IInclinedSection inclinedSection;
        private IGetLongitudinalForceFactorLogic getLongitudinalForceFactorLogic;

        private double crackLength;

        public ConcreteStrengthLogic(
            ISectionEffectiveness sectionEffectiveness,
            IInclinedSection inclinedSection,
            double longitudinalForce,
            IShiftTraceLogger? traceLogger)
        {
            this.sectionEffectiveness = sectionEffectiveness;
            this.inclinedSection = inclinedSection;
            this.longitudinalForce = longitudinalForce;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetShearStrength()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            InitializeStrategies();
            TraceLogger?.AddMessage($"Base shape factor = {sectionEffectiveness.BaseShapeFactor}, (dimensionless)");
            TraceLogger?.AddMessage($"Section shape factor = {sectionEffectiveness.ShapeFactor}, (dimensionless)");
            TraceLogger?.AddMessage($"Effective depth = {inclinedSection.EffectiveDepth}, (m)");
            crackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            TraceLogger?.AddMessage($"Crack length = {inclinedSection.EndCoord} - {inclinedSection.StartCoord} = {crackLength}, (m)");
            RestrictCrackLength();
            SetLongitudinalForce();
            double factorOfLongitudinalForce = getLongitudinalForceFactorLogic.GetFactor();
            TraceLogger?.AddMessage($"Factor of  longitudinal force = {factorOfLongitudinalForce}, (dimensionless)");
            double concreteMoment = sectionEffectiveness.BaseShapeFactor * sectionEffectiveness.ShapeFactor * inclinedSection.ConcreteTensionStrength * inclinedSection.WebWidth * inclinedSection.EffectiveDepth * inclinedSection.EffectiveDepth;
            double shearStrength = factorOfLongitudinalForce * concreteMoment / crackLength;
            TraceLogger?.AddMessage($"Shear strength of concrete = {shearStrength}, (N)");
            return shearStrength;
        }

        private void InitializeStrategies()
        {
            getLongitudinalForceFactorLogic ??= new GetLogitudinalForceFactorLogic(TraceLogger?.GetSimilarTraceLogger(100));
        }

        private void SetLongitudinalForce()
        {
            getLongitudinalForceFactorLogic.LongitudinalForce = longitudinalForce;
            getLongitudinalForceFactorLogic.InclinedSection = inclinedSection;
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
            if (crackLength < minCrackLength)
            {
                TraceLogger?.AddMessage($"Crack length c = {crackLength} is less than minimum crack length = {minCrackLength}");
                crackLength = minCrackLength;
                TraceLogger?.AddMessage($"Crack length = {crackLength}");
            }
            return;
        }
    }
}
