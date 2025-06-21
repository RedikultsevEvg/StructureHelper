using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.BeamShears.Logics;

namespace StructureHelperLogics.Models.BeamShears
{
    public class ConcreteStrengthLogic : IBeamShearStrenghLogic
    {
        private readonly ISectionEffectiveness sectionEffectiveness;
        private readonly IInclinedSection inclinedSection;
        

        private double crackLength;

        public ConcreteStrengthLogic(
            ISectionEffectiveness sectionEffectiveness,
            IInclinedSection inclinedSection,
            IShiftTraceLogger? traceLogger)
        {
            this.sectionEffectiveness = sectionEffectiveness;
            this.inclinedSection = inclinedSection;
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
            TraceLogger?.AddMessage($"Absolute crack length c = {inclinedSection.EndCoord} - {inclinedSection.StartCoord} = {crackLength}, (m)");
            TraceLogger?.AddMessage($"Relative crack length c/d = {crackLength} / {inclinedSection.EffectiveDepth} = {crackLength/ inclinedSection.EffectiveDepth},(dimensionless)");
            RestrictCrackLength();


            double concreteMoment = 
                sectionEffectiveness.BaseShapeFactor
                * sectionEffectiveness.ShapeFactor 
                * inclinedSection.ConcreteTensionStrength 
                * inclinedSection.WebWidth 
                * inclinedSection.EffectiveDepth 
                * inclinedSection.EffectiveDepth;
            double shearStrength = concreteMoment / crackLength;
            TraceLogger?.AddMessage($"Shear strength of concrete = {sectionEffectiveness.BaseShapeFactor} * {sectionEffectiveness.ShapeFactor} * {inclinedSection.ConcreteTensionStrength} * {inclinedSection.WebWidth} * {inclinedSection.EffectiveDepth} ^ 2 / {crackLength} = {shearStrength}, (N)");
            return shearStrength;
        }

        private void InitializeStrategies()
        {
            
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
