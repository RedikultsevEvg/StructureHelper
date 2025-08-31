using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Units;

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
            this.sectionEffectiveness = sectionEffectiveness
                ?? throw new StructureHelperException("Section effectiveness cannot be null.");
            this.inclinedSection = inclinedSection
                ?? throw new StructureHelperException("Inclined section cannot be null.");
            TraceLogger = traceLogger;
        }

        public double CalculateShearStrength()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            ValidateInput();
            TraceLogger?.AddMessage($"Section effectiveness base shape factor phi_b2 = {sectionEffectiveness.BaseShapeFactor}{UnitsOfSI.Dimensionless}");
            TraceLogger?.AddMessage($"Section effectiveness shape factor phi_shp = {sectionEffectiveness.ShapeFactor}{UnitsOfSI.Dimensionless}");
            TraceLogger?.AddMessage($"Concrete tension strength Rbt = {inclinedSection.ConcreteTensionStrength}{UnitsOfSI.Pa}");
            TraceLogger?.AddMessage($"Design width of cross-section b = {inclinedSection.WebWidth}{UnitsOfSI.Meters}");
            TraceLogger?.AddMessage($"Effective depth of cross-section ho = {inclinedSection.EffectiveDepth}{UnitsOfSI.Meters}");
            double rawCrackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            TraceLogger?.AddMessage($"Inclination section length c = {inclinedSection.EndCoord} - {inclinedSection.StartCoord} = {rawCrackLength}{UnitsOfSI.Meters}");
            double crackLength = RestrictCrackLength(rawCrackLength);
            double concreteMoment = CalculateConcreteMoment();
            double shearStrength = concreteMoment / crackLength;

            TraceLogger?.AddMessage($"Shear strength = Mb / c = {concreteMoment}{UnitsOfSI.NewtonMeters} / {crackLength}{UnitsOfSI.Meters} = {shearStrength}{UnitsOfSI.Newtons}");
            return shearStrength;
        }

        private double CalculateConcreteMoment()
        {
            double concreteMoment = sectionEffectiveness.BaseShapeFactor
                        * sectionEffectiveness.ShapeFactor
                        * inclinedSection.ConcreteTensionStrength
                        * inclinedSection.WebWidth
                        * inclinedSection.EffectiveDepth
                        * inclinedSection.EffectiveDepth;
            TraceLogger?.AddMessage($"Concrete moment Mb = {sectionEffectiveness.BaseShapeFactor} * {sectionEffectiveness.ShapeFactor} * {inclinedSection.ConcreteTensionStrength}{UnitsOfSI.Pa} * {inclinedSection.WebWidth}{UnitsOfSI.Meters} * ({inclinedSection.EffectiveDepth}{UnitsOfSI.Meters})^2 = {concreteMoment}{UnitsOfSI.NewtonMeters}");
            return concreteMoment;
        }

        private double RestrictCrackLength(double sourceLength)
        {
            double length = sourceLength;
            double maxLength = sectionEffectiveness.MaxCrackLengthRatio * inclinedSection.EffectiveDepth;
            double minLength = sectionEffectiveness.MinCrackLengthRatio * inclinedSection.EffectiveDepth;

            if (length > maxLength)
            {
                TraceLogger?.AddMessage($"Max design crack length cmax = {sectionEffectiveness.MaxCrackLengthRatio} * {inclinedSection.EffectiveDepth}{UnitsOfSI.Meters} = {maxLength}{UnitsOfSI.Meters}. Crack length has been reduced from {length}{UnitsOfSI.Meters} to {maxLength}{UnitsOfSI.Meters}");
                return maxLength;
            }
            if (length < minLength)
            {
                TraceLogger?.AddMessage($"Min design crack length cmin = {sectionEffectiveness.MinCrackLengthRatio} * {inclinedSection.EffectiveDepth}{UnitsOfSI.Meters} = {minLength}{UnitsOfSI.Meters}. Crack length has been increased from {length}{UnitsOfSI.Meters} to {minLength}{UnitsOfSI.Meters}");
                return minLength;
            }
            return length;
        }

        private void ValidateInput()
        {
            if (inclinedSection.EffectiveDepth <= 0)
                throw new StructureHelperException("Effective depth must be greater than zero.");
            if (inclinedSection.WebWidth <= 0)
                throw new StructureHelperException("Width of cross-section must be greater than zero.");
        }
    }

}
