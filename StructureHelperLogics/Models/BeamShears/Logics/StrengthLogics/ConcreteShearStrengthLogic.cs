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
        private IGetCrackLengthLogic restrictCrackLengthLogic;
        private readonly IGetShapeFactorLogic getShapeFactorLogic;
        private IGetCrackLengthLogic RestrictCrackLengthLogic => restrictCrackLengthLogic ??= new RestrictInclinedCrackSP63Logic(sectionEffectiveness, inclinedSection, TraceLogger);

        public IShiftTraceLogger? TraceLogger { get; set; }

        public ConcreteShearStrengthLogic(
            ISectionEffectiveness sectionEffectiveness,
            IInclinedSection inclinedSection,
            IGetShapeFactorLogic getShapeFactorLogic,
            IShiftTraceLogger? traceLogger = null)
        {
            this.sectionEffectiveness = sectionEffectiveness
                ?? throw new StructureHelperException("Section effectiveness cannot be null.");
            this.inclinedSection = inclinedSection
                ?? throw new StructureHelperException("Inclined section cannot be null.");
            this.getShapeFactorLogic = getShapeFactorLogic;
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
            double inclinedCrackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            TraceLogger?.AddMessage($"Inclination section length c = {inclinedSection.EndCoord} - {inclinedSection.StartCoord} = {inclinedCrackLength}{UnitsOfSI.Meters}");
            inclinedCrackLength = RestrictCrackLengthLogic.GetCrackLength(inclinedCrackLength);
            double concreteMoment = CalculateConcreteMoment();
            double shearStrength = concreteMoment / inclinedCrackLength;

            TraceLogger?.AddMessage($"Shear strength = Mb / c = {concreteMoment}{UnitsOfSI.NewtonMeters} / {inclinedCrackLength}{UnitsOfSI.Meters} = {shearStrength}{UnitsOfSI.Newtons}");
            return shearStrength;
        }

        private double CalculateConcreteMoment()
        {
            double shapeFactor = getShapeFactorLogic.GetShapeFactor();
            double concreteMoment = sectionEffectiveness.BaseShapeFactor
                        * sectionEffectiveness.ShapeFactor
                        * shapeFactor
                        * inclinedSection.ConcreteTensionStrength
                        * inclinedSection.WebWidth
                        * inclinedSection.EffectiveDepth
                        * inclinedSection.EffectiveDepth;
            TraceLogger?.AddMessage($"Concrete moment Mb = {sectionEffectiveness.BaseShapeFactor} * {sectionEffectiveness.ShapeFactor} * {shapeFactor} * {inclinedSection.ConcreteTensionStrength}{UnitsOfSI.Pa} * {inclinedSection.WebWidth}{UnitsOfSI.Meters} * ({inclinedSection.EffectiveDepth}{UnitsOfSI.Meters})^2 = {concreteMoment}{UnitsOfSI.NewtonMeters}");
            return concreteMoment;
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
