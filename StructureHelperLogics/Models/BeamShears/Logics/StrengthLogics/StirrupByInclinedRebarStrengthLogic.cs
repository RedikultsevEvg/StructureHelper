using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByInclinedRebarStrengthLogic : IBeamShearStrengthLogic
    {
        const double stirrupEffectivenessFactor = 0.75;
        private readonly IStirrupByInclinedRebar inclinedRebar;
        private readonly IInclinedSection inclinedSection;
        private double angleInRad;
        private double rebarStartPoint;
        private double rebarHeight;
        private double rebarEndPoint;
        private double rebarTrueStartPoint;
        private double rebarTrueEndPoint;
        private IRebarSectionStrengthLogic rebarSectionStrengthLogic;
        private IInterpolateValueLogic interpolationLogic;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public StirrupByInclinedRebarStrengthLogic(
            IInclinedSection inclinedSection,
            IStirrupByInclinedRebar inclinedRebar,
            IShiftTraceLogger traceLogger) : this(
                inclinedSection,
                inclinedRebar,
                new RebarSectionStrengthLogic(),
                new InterpolateValueLogic(),
                traceLogger
                ) { }

        public StirrupByInclinedRebarStrengthLogic(
            IInclinedSection inclinedSection,
            IStirrupByInclinedRebar inclinedRebar,
            IRebarSectionStrengthLogic rebarSectionStrengthLogic,
            IInterpolateValueLogic interpolationLogic,
            IShiftTraceLogger? traceLogger)
        {
            this.inclinedSection = inclinedSection;
            this.inclinedRebar = inclinedRebar;
            this.rebarSectionStrengthLogic = rebarSectionStrengthLogic;
            this.interpolationLogic = interpolationLogic;
            TraceLogger = traceLogger;
        }

        public double CalculateShearStrength()
        {
            GetGeometry();
            if (inclinedSection.StartCoord > rebarEndPoint)
            {
                TraceLogger?.AddMessage($"Inclined section start point coordinate X = {inclinedSection.StartCoord} is greater than inclined rebar end point X = {rebarEndPoint}, inclined rebar has been ignored");
                return 0.0;
            }
            if (inclinedSection.EndCoord < rebarStartPoint)
            {
                TraceLogger?.AddMessage($"Inclined section end point coordinate X = {inclinedSection.EndCoord} is less than inclined rebar start point X = {rebarStartPoint}, inclined rebar has been ignored");
                return 0.0;
            }
            if (inclinedSection.StartCoord > rebarTrueEndPoint & inclinedSection.StartCoord < rebarEndPoint)
            {
                TraceLogger?.AddMessage($"Inclined section start point coordinate X = {inclinedSection.StartCoord} is in end transfer zone");
                return GetEndTransferValue();
            }
            if (inclinedSection.EndCoord > rebarStartPoint && inclinedSection.EndCoord < rebarTrueStartPoint)
            {
                TraceLogger?.AddMessage($"Inclined section end point coordinate X = {inclinedSection.EndCoord} is in start transfer zone");
                return GetStartTransferValue();
            }
            TraceLogger?.AddMessage($"Inclined section with start point coordinate Xstart = {inclinedSection.StartCoord}(m) and end point Xend = {inclinedSection.EndCoord}(m) intersects inclined rebar in main zone with Xstart = {rebarTrueStartPoint}(m) and Xend = {rebarTrueEndPoint}(m)");
            return GetInclinedRebarStrength();
        }

        private double GetStartTransferValue()
        {
            interpolationLogic.X1 = rebarStartPoint;
            interpolationLogic.X2 = rebarTrueStartPoint;
            interpolationLogic.Y1 = 0.0;
            interpolationLogic.Y2 = GetInclinedRebarStrength();
            interpolationLogic.KnownValueX = inclinedSection.EndCoord;
            return interpolationLogic.GetValueY();
        }

        private double GetEndTransferValue()
        {
            interpolationLogic.X1 = rebarTrueEndPoint;
            interpolationLogic.X2 = rebarEndPoint;
            interpolationLogic.Y1 = GetInclinedRebarStrength();
            interpolationLogic.Y2 = 0.0;
            interpolationLogic.KnownValueX = inclinedSection.StartCoord;
            return interpolationLogic.GetValueY();
        }

        private double GetInclinedRebarStrength()
        {
            rebarSectionStrengthLogic.RebarStrengthFactor = 0.8;
            rebarSectionStrengthLogic.MaxRebarStrength = 3e8;
            rebarSectionStrengthLogic.LimitState = LimitStates.ULS;
            rebarSectionStrengthLogic.CalcTerm = CalcTerms.ShortTerm;
            rebarSectionStrengthLogic.TraceLogger = TraceLogger;
            rebarSectionStrengthLogic.RebarSection = inclinedRebar.RebarSection;

            double rebarStrength = rebarSectionStrengthLogic.GetRebarMaxTensileForce();
            double inclinedRebarStrength = stirrupEffectivenessFactor * rebarStrength * Math.Sin(angleInRad) * inclinedRebar.LegCount;
            TraceLogger?.AddMessage($"Inclined rebar Name = {inclinedRebar.Name}, start point {rebarStartPoint}(m), end point {rebarEndPoint}(m), angle of inclination {inclinedRebar.AngleOfInclination}(deg), number of legs {inclinedRebar.LegCount}");
            TraceLogger?.AddMessage($"Inclined rebar effectiveness factor fi_sw = {stirrupEffectivenessFactor}(dimensionless)");
            TraceLogger?.AddMessage($"Force in inclined rebar = fi_sw * Fsw * sin(alpha) * n = {stirrupEffectivenessFactor} * {rebarStrength}(N) * sin({inclinedRebar.AngleOfInclination}) * {inclinedRebar.LegCount} = {inclinedRebarStrength}(N)");
            return inclinedRebarStrength;
        }

        private void GetGeometry()
        {
            double transferLength = Math.Max(inclinedRebar.TransferLength, 0.01);
            angleInRad = inclinedRebar.AngleOfInclination / 180 * Math.PI;
            rebarStartPoint = inclinedRebar.StartCoordinate;
            rebarHeight = inclinedSection.EffectiveDepth - inclinedRebar.CompressedGap;
            rebarEndPoint = rebarStartPoint + rebarHeight / Math.Tan(angleInRad);
            rebarTrueStartPoint = rebarStartPoint + transferLength;
            rebarTrueEndPoint = rebarEndPoint - transferLength;
            if (rebarTrueStartPoint >= rebarTrueEndPoint)
            {
                throw new StructureHelperException("Transfer zone in inclined rebar is too big");
            }
        }
    }
}
