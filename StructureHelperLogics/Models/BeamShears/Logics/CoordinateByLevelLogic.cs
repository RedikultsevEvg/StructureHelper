using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class CoordinateByLevelLogic : ICoordinateByLevelLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }

        public CoordinateByLevelLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        /// <inheritdoc/>
        public double GetCoordinate(double startCoord, double endCoord, double relativeLevel)
        {
            CheckRelativeLevel(relativeLevel);
            double delta = endCoord - startCoord;
            double coordinate = startCoord + delta * (relativeLevel + 0.5d);
            return coordinate;
        }

        private void CheckRelativeLevel(double relativeLevel)
        {
            if (relativeLevel > 0.5d)
            {
                string errorString = ErrorStrings.IncorrectValue + ": relative level must not be greater than 0.5";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            if (relativeLevel < -0.5d)
            {
                string errorString = ErrorStrings.IncorrectValue + ": relative level must not be less than -0.5";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }
    }
}
