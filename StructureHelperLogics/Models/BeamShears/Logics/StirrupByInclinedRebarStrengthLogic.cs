using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByInclinedRebarStrengthLogic : IBeamShearStrenghLogic
    {
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
            double inclinedRebarStrength = rebarStrength * Math.Sin(angleInRad) * inclinedRebar.LegCount;
            TraceLogger?.AddMessage($"Inclinated rebar {inclinedRebar.Name}, start point {rebarStartPoint}(m), end point {rebarEndPoint}(m), angle of inclination {inclinedRebar.AngleOfInclination}(deg), number of legs {inclinedRebar.LegCount}");
            TraceLogger?.AddMessage($"Force in inclined rebar = {rebarStrength}(N) * sin({inclinedRebar.AngleOfInclination}) * {inclinedRebar.LegCount} = {inclinedRebarStrength}(N)");
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
        }
    }
}
