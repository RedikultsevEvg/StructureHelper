using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public class GetLongitudinalForceFactorLogic : IGetLongitudinalForceFactorLogic
    {
        private const double sndRatioInCompression = 0.5;
        private const double maxFactor = 1.25;

        private double sectionArea;
        private ShiftTraceLogger localTraceLogger;
        private ICheckEntityLogic<IInclinedSection> checkInclinedSectionLogic;
        private IGetReducedAreaLogic getReducedAreaLogic;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public IInclinedSection InclinedSection { get; set; }
        public double LongitudinalForce { get; set; }

        public GetLongitudinalForceFactorLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public GetLongitudinalForceFactorLogic(
            ICheckEntityLogic<IInclinedSection> checkInclinedSectionLogic,
            IGetReducedAreaLogic getReducedAreaLogic,
            IShiftTraceLogger? traceLogger)
        {
            this.checkInclinedSectionLogic = checkInclinedSectionLogic;
            this.getReducedAreaLogic = getReducedAreaLogic;
            TraceLogger = traceLogger;
        }

        public double GetFactor()
        {
            localTraceLogger = new();
            localTraceLogger.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            localTraceLogger.AddMessage("Logic of calculating of factor of influence of longitudinal force according to SP 63.13330.2018");
            Check();
            double result;
            if (LongitudinalForce == 0)
            {
                localTraceLogger.AddMessage("Longitudinal force is zero", TraceLogStatuses.Service);
                result = 1;
            }
            else
            {
                result = GetFactorByForce();
            }
            localTraceLogger.TraceLoggerEntries.ForEach(x => TraceLogger?.TraceLoggerEntries.Add(x));
            return result;
        }

        private double GetFactorByForce()
        {
            double result;
            getReducedAreaLogic ??= new GetReducedAreaLogicSP63_2018_rev3(LongitudinalForce, InclinedSection, localTraceLogger);
            sectionArea = getReducedAreaLogic.GetArea();
            if (LongitudinalForce < 0)
            {
                localTraceLogger.AddMessage($"Longitudinal force N={LongitudinalForce}(N) is negative (compression)", TraceLogStatuses.Service);
                result = GetNegForceResult();
            }
            else
            {
                localTraceLogger.AddMessage($"Longitudinal force N={LongitudinalForce}(N) is positive (tension)", TraceLogStatuses.Service);
                result = GetPosForceResult();
            }
            return result;
        }

        private void Check()
        {
            checkInclinedSectionLogic ??= new CheckInclinedSectionLogic(localTraceLogger);
            checkInclinedSectionLogic.Entity = InclinedSection;
            if (checkInclinedSectionLogic.Check() == false)
            {
                string errorString = checkInclinedSectionLogic.CheckResult;
                localTraceLogger.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }

        private double GetPosForceResult()
        {
            double stressInConcrete = LongitudinalForce / sectionArea;
            localTraceLogger.AddMessage($"Average stress in concrete (positive in tension) Sigma = {LongitudinalForce}(N) / {sectionArea}(m^2) = {stressInConcrete}(Pa)");
            double concreteStrength = InclinedSection.ConcreteTensionStrength;
            localTraceLogger.AddMessage($"Concrete strength Rbt = {concreteStrength}(Pa)");
            double stressRatio = stressInConcrete / concreteStrength;
            localTraceLogger.AddMessage($"Stress ratio rt = {stressInConcrete} / {concreteStrength} = {stressRatio}(dimensionless)");
            double factor = 1 - 0.5 * stressRatio;
            factor = Math.Max(factor, 0);
            localTraceLogger.AddMessage($"Factor value fi_n = {factor}(dimensionless)");
            return factor;
        }

        private double GetNegForceResult()
        {
            double stressInConcrete = (-1) * LongitudinalForce / sectionArea;
            localTraceLogger.AddMessage($"Average stress in concrete (positive in compression) Sigma = {LongitudinalForce}(N) / {sectionArea}(m^2) = {stressInConcrete}(Pa)");
            double concreteStrength = InclinedSection.ConcreteCompressionStrength;
            localTraceLogger.AddMessage($"Concrete strength Rb = {concreteStrength}(Pa)");
            double stressRatio = stressInConcrete / concreteStrength;
            localTraceLogger.AddMessage($"Stress ratio rc = {stressInConcrete} / {concreteStrength} = {stressRatio}(dimensionless)");
            double factor;
            double fstRatioInCompression = maxFactor-1;
            if (stressRatio < fstRatioInCompression)
            {
                localTraceLogger.AddMessage($"Stress ratio rc = {stressRatio} < {fstRatioInCompression}");
                factor = 1 + stressRatio;
            }
            else if (stressRatio > sndRatioInCompression)
            {
                double stressFactor = maxFactor / (1 - sndRatioInCompression);
                factor = stressFactor * (1 - stressRatio);
                factor = Math.Max(factor, 0);
            }
            else
            {
                factor = maxFactor;
            }
            localTraceLogger.AddMessage($"Factor value fi_n = {factor}(dimensionless)");
            return factor;
        }
    }
}
