using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public class GetLogitudinalForceFactorLogic : IGetLongitudinalForceFactorLogic
    {
        private const double fstRatioInCompression = 1.25;
        private const double sndRationInCompression = 0.75;
        private double sectionArea;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public IInclinedSection InclinedSection { get; set; }
        public double LongitudinalForce { get; set; }

        public GetLogitudinalForceFactorLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }


        public double GetFactor()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Logic of calculating of factor of influence of longitudinal force according to SP 63.13330.2018");
            Check();
            if (LongitudinalForce == 0)
            {
                TraceLogger?.AddMessage("Longitudinal force is zero", TraceLogStatuses.Service);
                return 1;
            }
            sectionArea = InclinedSection.WebWidth * InclinedSection.FullDepth;
            TraceLogger?.AddMessage($"Area of cross-section Ac = {InclinedSection.WebWidth} * {InclinedSection.FullDepth} = {sectionArea}(m^2)");
            if (LongitudinalForce < 0)
            {
                TraceLogger?.AddMessage($"Longitudinal force N={LongitudinalForce}(N) is negative (compression)", TraceLogStatuses.Service);
                return GetNegForceResult();
            }
            else
            {
                TraceLogger?.AddMessage("Longitudinal force N={LongitudinalForce}(N) is positive (tension)", TraceLogStatuses.Service);
                return GetPosForceResult();
            }
        }

        private void Check()
        {
            if (InclinedSection is null)
            {
                string errorString = ErrorStrings.DataIsInCorrect + "Inclined section is null";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            if (InclinedSection.WebWidth <= 0 || InclinedSection.FullDepth <= 0)
            {
                string errorString = ErrorStrings.DataIsInCorrect + $"Inclined section width = {InclinedSection.WebWidth}(m), and full depth = {InclinedSection.FullDepth}, but both of them must be greater than zero";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }

        private double GetPosForceResult()
        {
            double stressInConcrete = LongitudinalForce / sectionArea;
            TraceLogger?.AddMessage($"Average stress in concrete (positive in tension) Sigma = {LongitudinalForce}(N) / {sectionArea}(m^2) = {stressInConcrete}(Pa)");
            double concreteStrength = InclinedSection.ConcreteTensionStrength;
            TraceLogger?.AddMessage($"Concrete strength Rbt = {concreteStrength}(Pa)");
            double stressRatio = stressInConcrete / concreteStrength;
            TraceLogger?.AddMessage($"Stress ratio rt = {stressInConcrete} / {concreteStrength} = {stressRatio}(dimensionless)");
            double factor = 1 - 0.5 * stressRatio;
            factor = Math.Max(factor, 0);
            TraceLogger?.AddMessage($"Factor value fi_n = {factor}(dimensionless)");
            return factor;
        }

        private double GetNegForceResult()
        {
            double stressInConcrete = (-1) * LongitudinalForce / sectionArea;
            TraceLogger?.AddMessage($"Average stress in concrete (positive in compression) Sigma = {LongitudinalForce}(N) / {sectionArea}(m^2) = {stressInConcrete}(Pa)");
            double concreteStrength = InclinedSection.ConcreteCompressionStrength;
            TraceLogger?.AddMessage($"Concrete strength Rb = {concreteStrength}(Pa)");
            double stressRatio = stressInConcrete / concreteStrength;
            TraceLogger?.AddMessage($"Stress ratio rc = {stressInConcrete} / {concreteStrength} = {stressRatio}(dimensionless)");
            double factor;
            if (stressRatio < fstRatioInCompression)
            {
                TraceLogger?.AddMessage($"Stress ratio rc = {stressRatio} < {fstRatioInCompression}");
                factor = 1 + stressRatio;
            }
            else if (stressRatio > sndRationInCompression)
            {
                factor = 5 * (1 - stressRatio);
                factor = Math.Max(factor, 0);
            }
            else
            {
                factor = 1;
            }
            TraceLogger?.AddMessage($"Factor value fi_n = {factor}(dimensionless)");
            return factor;
        }
    }
}
