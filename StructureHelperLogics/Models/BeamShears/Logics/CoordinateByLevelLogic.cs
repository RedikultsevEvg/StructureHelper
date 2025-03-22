using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class CoordinateByLevelLogic : ICoordinateByLevelLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }

        public CoordinateByLevelLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

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
