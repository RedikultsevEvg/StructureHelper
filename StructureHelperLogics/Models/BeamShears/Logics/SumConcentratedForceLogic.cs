using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;

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

        public double GetSumShearForce(IBeamSpanLoad beamShearLoad, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            if (beamShearLoad is IConcentratedForce concentratedForce)
            {
                InitializeStrategies();
                double sumForce = GetConcentratedForceSum(concentratedForce, startCoord, endCoord);
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

        private double GetConcentratedForceSum(IConcentratedForce concentratedForce, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage($"Concentrated force Name = {concentratedForce.Name}, Value = {concentratedForce.ForceValue}(N/m) ");
            if (concentratedForce.ForceCoordinate > endCoord)
            {
                TraceLogger?.AddMessage($"Force coordinate {concentratedForce.ForceCoordinate}(m) is bigger than section end {endCoord}(m), so total load is zero");
                return 0;
            }
            return GetConcentratedForce(concentratedForce, startCoord, endCoord);
        }

        private double GetConcentratedForce(IConcentratedForce concentratedForce, double startCoord, double endCoord)
        {
            double totalLoad;
            double limitCoordinate = startCoord;
            if (concentratedForce.ForceCoordinate >= startCoord)
            {
                limitCoordinate = coordinateByLevelLogic.GetCoordinate(startCoord, endCoord, concentratedForce.RelativeLoadLevel);
            }
            if (concentratedForce.ForceCoordinate < limitCoordinate)
            {
                totalLoad = concentratedForce.ForceValue * concentratedForce.LoadRatio;
                TraceLogger?.AddMessage($"Total load Q,tot = {concentratedForce.ForceValue}(N) * {concentratedForce.LoadRatio} = {totalLoad}(N)");
            }
            else
            {
                TraceLogger?.AddMessage($"Force coordinate {concentratedForce.ForceCoordinate}(m) is bigger than limit coordinate {limitCoordinate}(m), so total load is zero");
                totalLoad = 0d;
            }
            return totalLoad;
        }
    }
}
