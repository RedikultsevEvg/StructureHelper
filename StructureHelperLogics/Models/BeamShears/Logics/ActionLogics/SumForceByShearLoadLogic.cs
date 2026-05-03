using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.BeamShears.Logics.ActionLogics;

namespace StructureHelperLogics.Models.BeamShears
{
    public class SumForceByShearLoadLogic : ISumForceByShearLoadLogic
    {
        private ISumForceByShearLoadLogic sumDistributedLoadLogic;
        private ISumForceByShearLoadLogic sumConcentratedForceLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }

        public SumForceByShearLoadLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public SumForceByShearLoadLogic(
            ISumForceByShearLoadLogic sumDistributedLoadLogic,
            ISumForceByShearLoadLogic sumConcentratedForceLogic,
            IShiftTraceLogger? traceLogger)
        {
            this.sumDistributedLoadLogic = sumDistributedLoadLogic;
            this.sumConcentratedForceLogic = sumConcentratedForceLogic;
            TraceLogger = traceLogger;
        }

        public IForceTuple GetSumShearForce(IBeamSpanLoad beamShearLoad, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            if (beamShearLoad is IDistributedLoad distributedLoad)
            {
                TraceLogger?.AddMessage($"Load is uniformly distributed load");
                IForceTuple sumDistributed = GetDistributedLoadSum(distributedLoad, startCoord, endCoord);
                return sumDistributed;
            }
            else if (beamShearLoad is IConcentratedForce concenratedForce)
            {
                TraceLogger?.AddMessage($"Load is concentrated force");
                return GetConcentratedForceSum(concenratedForce, startCoord, endCoord);
            }
            else if (beamShearLoad is ITrapezoidDistributedLoad trapezoid)
            {
                TraceLogger?.AddMessage($"Load is concentrated force");
                return GetTrapezoidSum(trapezoid, startCoord, endCoord);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(beamShearLoad));
            }
        }

        private IForceTuple GetTrapezoidSum(ITrapezoidDistributedLoad trapezoid, double startCoord, double endCoord)
        {
            SumTrapezoidDistributedLoadLogic logic = new(TraceLogger);
            IForceTuple sumForce = logic.GetSumShearForce(trapezoid, startCoord, endCoord);
            TraceLogger?.AddMessage($"Sum of trapezoid distributed load Qud = {sumForce.Qy}(N)");
            return sumForce;
        }

        private IForceTuple GetConcentratedForceSum(IConcentratedForce concentratedForce, double startCoord, double endCoord)
        {
            sumConcentratedForceLogic ??= new SumConcentratedForceLogic(TraceLogger) { LimitState = LimitState, CalcTerm = CalcTerm};
            IForceTuple sumForce = sumConcentratedForceLogic.GetSumShearForce(concentratedForce, startCoord, endCoord);
            TraceLogger?.AddMessage($"Sum of concentrated force Qcf = {sumForce.Qy}(N)");
            return sumForce;
        }



        private IForceTuple GetDistributedLoadSum(IDistributedLoad distributedLoad, double startCoord, double endCoord)
        {
            sumDistributedLoadLogic ??= new SumDistributedLoadLogic(TraceLogger) { LimitState = LimitState, CalcTerm = CalcTerm };
            IForceTuple sumForce = sumDistributedLoadLogic.GetSumShearForce(distributedLoad, startCoord, endCoord);
            TraceLogger?.AddMessage($"Sum of uniformly distributed load Qud = {sumForce.Qy}(N)");
            return sumForce;
        }

    }
}
