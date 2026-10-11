using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Units;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetVerticalTShapeFactorSHLogic : IGetShapeFactorLogic
    {
        private const double flangeEffectivenessFactor = 0.5d;
        private const double maxEffectiveFlangeWidthFactor = 3d;
        private const double minTShapeConsideringFactor = 1.5d;
        private const double minFlangeHightFactor = 0.1d;
        private const double maxTShapeConsideringFactor = 3d;
        private const double maxFactorValue = 1.25d;
        private const double minWebWidth = 0.05d;
        private const double minEffectiveDepth = 0.05d;
        private double effectiveArea;
        private double inclinedSectionLength;
        private double effectiveDepth;
        private double webEffectiveArea;
        private double webWidth;
        private double flangeOffset;
        private double flangeWidth;
        private double effectiveFlangeArea;
        private double flangeHeight;
        private IInclinedSection inclinedSection { get; set; }
        private IVerticalTShape verticalTShape;
        private double factor;

        public GetVerticalTShapeFactorSHLogic(
            IInclinedSection inclinedSection,
            IVerticalTShape verticalTShape,
            IShiftTraceLogger? traceLogger = null)
        {
            CheckObject.ThrowIfNull(inclinedSection, "Inclined section cannot be null.");
            CheckObject.ThrowIfNull(verticalTShape);
            this.inclinedSection = inclinedSection;
            this.verticalTShape = verticalTShape;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }


        public double GetShapeFactor()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            Prepare();
            CalculateEffectiveCrossSectionArea();
            factor = effectiveArea / webEffectiveArea;
            factor = Math.Min(factor, maxFactorValue);
            TraceLogger?.AddMessage($"Shape factor phi_f'= {effectiveArea} / {webEffectiveArea} = {factor}{UnitsOfSI.Dimensionless}");
            return factor;
        }

        private void CalculateEffectiveCrossSectionArea()
        {
            webEffectiveArea = webWidth * effectiveDepth;
            TraceLogger?.AddMessage($"Web area Aeff={webEffectiveArea}{UnitsOfSI.SquareMeters}");
            double leftFlangeCantilever = (flangeWidth - webWidth) / 2d - flangeOffset;
            double rightFlangeCantilever = (flangeWidth - webWidth) / 2d + flangeOffset;
            effectiveFlangeArea = GetFlangeEffectiveArea(leftFlangeCantilever) + GetFlangeEffectiveArea(rightFlangeCantilever);
            TraceLogger?.AddMessage($"Effective flange area Aeff={effectiveFlangeArea}{UnitsOfSI.SquareMeters}");
            effectiveArea = webEffectiveArea;
            inclinedSectionLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            double minInclinedCrackLength = minTShapeConsideringFactor * effectiveDepth;
            if (inclinedSectionLength <= minInclinedCrackLength)
            {
                TraceLogger?.AddMessage($"Shape factor is zero due since shear span c = {inclinedSectionLength}{UnitsOfSI.Meters} less or equal than minimum shear span c_min = {minTShapeConsideringFactor} * {effectiveDepth} = {minInclinedCrackLength}");
                return;
            }
            else if (inclinedSectionLength >= maxTShapeConsideringFactor * effectiveDepth)
            {
                effectiveArea += effectiveFlangeArea;
                return;
            }
            else
            {
                double relativeShearSpan = inclinedSectionLength / effectiveDepth;
                TraceLogger?.AddMessage($"Relative shear span c/d={relativeShearSpan}{UnitsOfSI.Meters}");
                double consideredFlangeArea = effectiveFlangeArea / (maxTShapeConsideringFactor - minTShapeConsideringFactor) * (relativeShearSpan - minTShapeConsideringFactor);
                effectiveArea += consideredFlangeArea;
            }
        }

        private double GetFlangeEffectiveArea(double flangeCantilever)
        {
            if (flangeCantilever < 0d)
            {
                throw new StructureHelperException($"Flange cantilever must not be less than zero, but was ({flangeWidth} - {webWidth}) / 2 = {flangeCantilever}({UnitsOfSI.Meters})");
            }
            if (flangeHeight < minFlangeHightFactor * effectiveDepth) { return 0; }
            double effectiveArea;
            if (flangeCantilever > maxEffectiveFlangeWidthFactor * flangeHeight)
            {
                effectiveArea = maxEffectiveFlangeWidthFactor * flangeHeight * flangeHeight * flangeEffectivenessFactor;
            }
            else
            {
                effectiveArea = flangeCantilever * flangeHeight - flangeCantilever * flangeCantilever / maxEffectiveFlangeWidthFactor * flangeEffectivenessFactor;
            }
            return effectiveArea;
        }

        private void Prepare()
        {
            webWidth = verticalTShape.WebWidth;
            if (webWidth < minWebWidth)
            {
                throw new StructureHelperException($"Web width bf' = {webWidth} must not be less than minimum web width bf',min = {minWebWidth}({UnitsOfSI.Meters})");
            }
            TraceLogger?.AddMessage($"Web width bw={webWidth}{UnitsOfSI.Meters}");
            flangeOffset = verticalTShape.FlangeXOffset;
            effectiveDepth = inclinedSection.EffectiveDepth;
            if (effectiveDepth < minEffectiveDepth)
            {
                throw new StructureHelperException($"Effective depth d = {effectiveDepth} must not be less than minimum effective depth d,min = {minEffectiveDepth}({UnitsOfSI.Meters})");
            }
            flangeWidth = verticalTShape.FlangeWidth;
            TraceLogger?.AddMessage($"Flange width bf'={flangeWidth}{UnitsOfSI.Meters}");
            flangeHeight = verticalTShape.FlangeHeight;
            TraceLogger?.AddMessage($"Flange height bf'={flangeHeight}{UnitsOfSI.Meters}");
        }
    }
}
