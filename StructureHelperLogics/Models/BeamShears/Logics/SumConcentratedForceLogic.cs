using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class SumConcentratedForceLogic : ISumForceByShearLoadLogic
    {
        private ICoordinateByLevelLogic coordinateByLevelLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public SumConcentratedForceLogic(ICoordinateByLevelLogic coordinateByLevelLogic, IShiftTraceLogger? traceLogger)
        {
            this.coordinateByLevelLogic = coordinateByLevelLogic;
            TraceLogger = traceLogger;
        }

        public SumConcentratedForceLogic(IShiftTraceLogger? traceLogger)

        {
            TraceLogger = traceLogger;
        }

        public IForceTuple GetSumShearForce(IBeamSpanLoad beamShearLoad, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            if (beamShearLoad is IConcentratedForce concentratedForce)
            {
                InitializeStrategies();
                IForceTuple sumForce = GetConcentratedForceSum(concentratedForce, startCoord, endCoord);
                return sumForce;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(beamShearLoad) + ": beam shear load is not concentrated force");
            }
        }

        private void InitializeStrategies()
        {
            coordinateByLevelLogic ??= new CoordinateByLevelLogic(TraceLogger);
        }

        private IForceTuple GetConcentratedForceSum(IConcentratedForce concentratedForce, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage($"Concentrated force Name = {concentratedForce.Name}, Value = {concentratedForce.ForceValue.Qy}(N/m) ");
            if (concentratedForce.ForceCoordinate > endCoord)
            {
                TraceLogger?.AddMessage($"Force coordinate {concentratedForce.ForceCoordinate}(m) is bigger than section end {endCoord}(m), so total load is zero");
                return new ForceTuple(Guid.NewGuid());
            }
            return GetConcentratedForce(concentratedForce, startCoord, endCoord);
        }

        private IForceTuple GetConcentratedForce(IConcentratedForce concentratedForce, double startCoord, double endCoord)
        {
            IForceTuple totalLoad;
            double limitCoordinate = startCoord;
            if (concentratedForce.ForceCoordinate >= startCoord)
            {
                limitCoordinate = coordinateByLevelLogic.GetCoordinate(startCoord, endCoord, concentratedForce.RelativeLoadLevel);
            }
            if (concentratedForce.ForceCoordinate < limitCoordinate)
            {
                totalLoad = ForceTupleService.MultiplyTupleByFactor(concentratedForce.ForceValue, concentratedForce.LoadRatio);
                TraceLogger?.AddMessage($"Total load Q,tot = {concentratedForce.ForceValue.Qy}(N) * {concentratedForce.LoadRatio} = {totalLoad}(N)");
            }
            else
            {
                TraceLogger?.AddMessage($"Force coordinate {concentratedForce.ForceCoordinate}(m) is bigger than limit coordinate {limitCoordinate}(m), so total load is zero");
                totalLoad = new ForceTuple(Guid.NewGuid());
            }
            return totalLoad;
        }
    }
}
