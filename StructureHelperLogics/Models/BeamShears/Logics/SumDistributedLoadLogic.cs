using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class SumDistributedLoadLogic : ISumForceByShearLoadLogic
    {
        private ICoordinateByLevelLogic coordinateByLevelLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public SumDistributedLoadLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public SumDistributedLoadLogic(ICoordinateByLevelLogic coordinateByLevelLogic, IShiftTraceLogger? traceLogger)
        {
            this.coordinateByLevelLogic = coordinateByLevelLogic;
            TraceLogger = traceLogger;
        }

        public IForceTuple GetSumShearForce(IBeamSpanLoad beamShearLoad, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            if (beamShearLoad is IDistributedLoad distributedLoad)
            {
                InitializeStrategies();
                IForceTuple sumForce = GetDistributedLoadSum(distributedLoad, startCoord, endCoord);
                return sumForce;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(beamShearLoad) + ": beam shear load is not distributed load");
            }
        }

        private void InitializeStrategies()
        {
            coordinateByLevelLogic ??= new CoordinateByLevelLogic(TraceLogger);
        }

        private IForceTuple GetDistributedLoadSum(IDistributedLoad distributedLoad, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage($"Uniformly distributed load Name = {distributedLoad.Name}, Value = {distributedLoad.LoadValue.Qy}(N/m) ");
            double loadStartCoord = Math.Max(distributedLoad.StartCoordinate, 0d);
            if (loadStartCoord > endCoord)
            {
                TraceLogger?.AddMessage($"Load start coordinate {loadStartCoord}(m) is bigger than section end {endCoord}(m), so total load is zero");
                return new ForceTuple(Guid.NewGuid());
            }
            double endCoordByLevel = coordinateByLevelLogic.GetCoordinate(startCoord, endCoord, distributedLoad.RelativeLoadLevel);
            double loadEndCoord = Math.Min(distributedLoad.EndCoordinate, endCoordByLevel);
            double loadLength = loadEndCoord - loadStartCoord;
            TraceLogger?.AddMessage($"Total length L,tot = {loadEndCoord}(m) - {loadStartCoord}(m) = {loadLength}(m)");
            double sumFactor = distributedLoad.LoadRatio * loadLength;
            IForceTuple totalLoad = ForceTupleService.MultiplyTupleByFactor(distributedLoad.LoadValue, sumFactor);
            TraceLogger?.AddMessage($"Total load Q,tot = {distributedLoad.LoadValue.Qy}(N/m) * {distributedLoad.LoadRatio} * {loadLength}(m) = {totalLoad}(N)");
            return totalLoad;
        }
    }
}
