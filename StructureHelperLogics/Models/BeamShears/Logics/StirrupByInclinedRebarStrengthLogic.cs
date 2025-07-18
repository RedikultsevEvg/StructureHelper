using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByInclinedRebarStrengthLogic : IBeamShearStrenghLogic
    {
        const double stirrupEffectivenessFactor = 0.75;
        private readonly IStirrupByInclinedRebar inclinedRebar;
        private readonly IInclinedSection inclinedSection;
        private IRebarSectionStrengthLogic rebarSectionStrengthLogic;
        private double angleInRad;
        private double rebarStartPoint;
        private double rebarHeight;
        private double rebarEndPoint;
        private double rebarTrueStartPoint;
        private double rebarTrueEndPoint;
        private IInterpolateValueLogic interpolationLogic;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public StirrupByInclinedRebarStrengthLogic(IInclinedSection inclinedSection, IStirrupByInclinedRebar inclinedRebar, IShiftTraceLogger traceLogger)
        {
            this.inclinedSection = inclinedSection;
            this.inclinedRebar = inclinedRebar;
            TraceLogger = traceLogger;
        }


        public double GetShearStrength()
        {
            GetGeometry();
            if (inclinedSection.StartCoord > rebarEndPoint)
            {
                TraceLogger?.AddMessage($"Inclined section start point coordinate X = {inclinedSection.StartCoord} is greater than inclined rebar end point x = {rebarEndPoint}, inclined rebar has been ignored");
                return 0.0;
            }
            if (inclinedSection.EndCoord < rebarStartPoint)
            {
                TraceLogger?.AddMessage($"Inclined section end point coordinate X = {inclinedSection.EndCoord} is less than inclined rebar start point x = {rebarStartPoint}, inclined rebar has been ignored");
                return 0.0;
            }
            if (inclinedSection.StartCoord > rebarTrueEndPoint & inclinedSection.StartCoord < rebarEndPoint)
            {
                TraceLogger?.AddMessage($"Inclined section start point coordinate X = {inclinedSection.StartCoord} is in end transfer zone");
                return GetEndTransferValue();
            }
            if (inclinedSection.EndCoord > rebarStartPoint & inclinedSection.EndCoord < rebarTrueStartPoint)
            {
                TraceLogger?.AddMessage($"Inclined section end point coordinate X = {inclinedSection.EndCoord} is in start transfer zone");
                return GetStartTransferValue();
            }
            TraceLogger?.AddMessage($"Inclined section with start point coordinate Xstart = {inclinedSection.StartCoord}(m) and end point Xend = {inclinedSection.EndCoord}(m) intersects inclined rebar in main zone with Xstart = {rebarTrueStartPoint}(m) and Xend = {rebarTrueEndPoint}(m)");
            return GetInclinedRebarStrength();
        }

        private double GetStartTransferValue()
        {
            interpolationLogic = new InterpolateValueLogic()
            {
                X1 = rebarStartPoint,
                X2 = rebarTrueStartPoint,
                Y1 = 0.0,
                Y2 = GetInclinedRebarStrength(),
                KnownValueX = inclinedSection.EndCoord
            };
            return interpolationLogic.GetValueY();
        }

        private double GetEndTransferValue()
        {
            interpolationLogic = new InterpolateValueLogic()
            {
                X1 = rebarTrueEndPoint,
                X2 = rebarEndPoint,
                Y1 = GetInclinedRebarStrength(),
                Y2 = 0.0,
                KnownValueX = inclinedSection.StartCoord
            };
            return interpolationLogic.GetValueY();
        }

        private double GetInclinedRebarStrength()
        {
            rebarSectionStrengthLogic ??= new RebarSectionStrengthLogic()
            {
                RebarStrengthFactor = 0.8,
                MaxRebarStrength = 3e8,
                LimitState = LimitStates.ULS,
                CalcTerm = CalcTerms.ShortTerm,
                TraceLogger = TraceLogger,
            };
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
                throw new StructureHelperException("Transfer aone in inclined rebar is too big");
            }
        }
    }
}
