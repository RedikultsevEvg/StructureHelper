using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class GetDirectShearForceLogic : IGetDirectShearForceLogic
    {
        ISumForceByShearLoadLogic summaryForceLogic;

        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IBeamShearAxisAction AxisAction { get; private set; }
        public IInclinedSection InclinedSection { get; private set; }

        public GetDirectShearForceLogic(
            IBeamShearAxisAction axisAction,
            IInclinedSection inclinedSection, 
            IShiftTraceLogger? traceLogger)
        {
            AxisAction = axisAction;
            InclinedSection = inclinedSection;
            TraceLogger = traceLogger;
        }

        public GetDirectShearForceLogic(
            IBeamShearAxisAction axisAction,
            IInclinedSection inclinedSection,
            IShiftTraceLogger? traceLogger,
            ISumForceByShearLoadLogic summaryForceLogic)
        {
            this.summaryForceLogic = summaryForceLogic;
            AxisAction = axisAction;
            InclinedSection = inclinedSection;
            TraceLogger = traceLogger;
        }

        /// <inheritdoc/>
        public double CalculateShearForce()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            InitializeStrategies();
            double supportShearForce = AxisAction.SupportShearForce;
            TraceLogger?.AddMessage($"Shear force at support Qmax = {supportShearForce}(N)");
            TraceLogger?.AddMessage($"Start of inclined section a,start = {InclinedSection.StartCoord}(m)");
            TraceLogger?.AddMessage($"End of inclined section a,end = {InclinedSection.EndCoord}(m)");
            double summarySpanShearForce = AxisAction.ShearLoads
                .Sum(load => summaryForceLogic.GetSumShearForce(load, InclinedSection.StartCoord, InclinedSection.EndCoord));
            TraceLogger?.AddMessage($"Summary span force deltaQ = {summarySpanShearForce}(N)");
            double shearForce = supportShearForce + summarySpanShearForce;
            TraceLogger?.AddMessage($"Summary shear force at the end of inclined section Q = {shearForce}(N)");
            return shearForce;
        }

        private void InitializeStrategies()
        {
            summaryForceLogic ??= new SumForceByShearLoadLogic(TraceLogger);
        }
    }
}
