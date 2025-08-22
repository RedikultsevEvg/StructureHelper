using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.Models.BeamShears.Logics;

namespace StructureHelperLogics.Models.BeamShears
{
    public class ConcreteShearStrengthLogic : IBeamShearStrengthLogic
    {
        private readonly ISectionEffectiveness sectionEffectiveness;
        private readonly IInclinedSection inclinedSection;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public ConcreteShearStrengthLogic(
            ISectionEffectiveness sectionEffectiveness,
            IInclinedSection inclinedSection,
            IShiftTraceLogger? traceLogger = null)
        {
            this.sectionEffectiveness = sectionEffectiveness ?? throw new StructureHelperException("Section effectiveness cannot be null.");
            this.inclinedSection = inclinedSection ?? throw new StructureHelperException("Inclined section cannot be null.");
            TraceLogger = traceLogger;
        }

        public double CalculateShearStrength()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            ValidateInput();

            double rawCrackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            double crackLength = RestrictCrackLength(rawCrackLength);

            double concreteMoment = CalculateConcreteMoment();
            double shearStrength = concreteMoment / crackLength;

            TraceLogger?.AddMessage($"Shear strength = {concreteMoment} / {crackLength} = {shearStrength} {UnitsOfSI.Newtons}");
            return shearStrength;
        }

        private double CalculateConcreteMoment() =>
            sectionEffectiveness.BaseShapeFactor
            * sectionEffectiveness.ShapeFactor
            * inclinedSection.ConcreteTensionStrength
            * inclinedSection.WebWidth
            * inclinedSection.EffectiveDepth
            * inclinedSection.EffectiveDepth;

        private double RestrictCrackLength(double length)
        {
            double max = sectionEffectiveness.MaxCrackLengthRatio * inclinedSection.EffectiveDepth;
            double min = sectionEffectiveness.MinCrackLengthRatio * inclinedSection.EffectiveDepth;

            if (length > max)
            {
                TraceLogger?.AddMessage($"Crack length reduced from {length} to {max}");
                return max;
            }
            if (length < min)
            {
                TraceLogger?.AddMessage($"Crack length increased from {length} to {min}");
                return min;
            }
            return length;
        }

        private void ValidateInput()
        {
            if (inclinedSection.EffectiveDepth <= 0)
                throw new StructureHelperException("Effective depth must be greater than zero.");
        }
    }

}
