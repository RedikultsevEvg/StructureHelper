using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

namespace StructureHelperLogics.Models.BeamShears
{
    public class SumForceByShearLoadLogic : ISumForceByShearLoadLogic
    {
        private ISumForceByShearLoadLogic sumDistributedLoadLogic;
        private ISumForceByShearLoadLogic sumConcentratedForceLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }

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
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(beamShearLoad));
            }
        }

        private IForceTuple GetConcentratedForceSum(IConcentratedForce concenratedForce, double startCoord, double endCoord)
        {
            sumConcentratedForceLogic ??= new SumConcentratedForceLogic(TraceLogger);
            IForceTuple sumForce = sumConcentratedForceLogic.GetSumShearForce(concenratedForce, startCoord, endCoord);
            TraceLogger?.AddMessage($"Sum of concentrated force Qcf = {sumForce.Qy}(N)");
            return sumForce;
        }

        private IForceTuple GetDistributedLoadSum(IDistributedLoad distributedLoad, double startCoord, double endCoord)
        {
            sumDistributedLoadLogic ??= new SumDistributedLoadLogic(TraceLogger);
            IForceTuple sumForce = sumDistributedLoadLogic.GetSumShearForce(distributedLoad, startCoord, endCoord);
            TraceLogger?.AddMessage($"Sum of uniformly distributed load Qud = {sumForce.Qy}(N)");
            return sumForce;
        }

    }
}
