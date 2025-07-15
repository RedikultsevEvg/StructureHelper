using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.Materials;
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
            if (inclinedSection.StartCoord > rebarTrueEndPoint)
            {
                TraceLogger?.AddMessage($"Inclined section start point coordinate X = {inclinedSection.StartCoord} is greater than inclined rebar true end point x = {rebarTrueEndPoint}, inclined rebar has been ignored");
                return 0.0;
            }
            if (inclinedSection.EndCoord < rebarTrueStartPoint)
            {
                TraceLogger?.AddMessage($"Inclined section end point coordinate X = {inclinedSection.EndCoord} is less than inclined rebar true end point x = {rebarTrueStartPoint}, inclined rebar has been ignored");
                return 0.0;
            }
            return GetInclinedRebarStrength();
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
            angleInRad = inclinedRebar.AngleOfInclination / 180 * Math.PI;
            rebarStartPoint = inclinedRebar.StartCoordinate;
            rebarHeight = inclinedSection.EffectiveDepth - inclinedRebar.CompressedGap;
            rebarEndPoint = rebarStartPoint + rebarHeight * Math.Cos(angleInRad);
            rebarTrueStartPoint = rebarStartPoint + inclinedRebar.OffSet;
            rebarTrueEndPoint = rebarEndPoint - inclinedRebar.OffSet;
        }
    }
}
