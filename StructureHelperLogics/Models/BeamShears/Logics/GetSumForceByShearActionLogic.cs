using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetSumForceByShearActionLogic : IGetSumForceByShearActionLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }

        public GetSumForceByShearActionLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public double GetSumShearForce(IBeamShearLoad beamShearAction, double startCoord, double endCoord)
        {
            if (beamShearAction is IDistributedLoad distributedLoad)
            {
                return GetDistributedLoadSum(distributedLoad, startCoord, endCoord);
            }
            else if (beamShearAction is IConcentratedForce concenratedForce)
            {
                return GetConcentratedForceSum(concenratedForce, startCoord, endCoord);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(beamShearAction));
            }
        }

        private double GetConcentratedForceSum(IConcentratedForce concentratedForce, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage($"Concentrated force Name = {concentratedForce.Name}, Value = {concentratedForce.ForceValue}(N/m) ");
            if (concentratedForce.ForceCoordinate > endCoord)
            {
                TraceLogger?.AddMessage($"Force coordinate {concentratedForce.ForceCoordinate}(m) is bigger than section end {endCoord}(m), so total load is zero");
                return 0;
            }
            double totalLoad;
            double limitCoordinate = startCoord;
            if (concentratedForce.ForceCoordinate >= startCoord)
            {
                limitCoordinate = GetCoordinateByLevel(startCoord, endCoord, concentratedForce.RelativeLoadLevel);
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

        private double GetDistributedLoadSum(IDistributedLoad distributedLoad, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage($"Uniformly distributed load Name = {distributedLoad.Name}, Value = {distributedLoad.LoadValue}(N/m) ");
            double loadStartCoord = Math.Max(distributedLoad.StartCoordinate, 0d);
            if (loadStartCoord > endCoord)
            {
                TraceLogger?.AddMessage($"Load start coordinate {loadStartCoord}(m) is bigger than section end {endCoord}(m), so total load is zero");
                return 0d;
            }
            double endCoordByLevel = GetCoordinateByLevel(startCoord, endCoord, distributedLoad.RelativeLoadLevel);
            double loadEndCoord = Math.Min(distributedLoad.EndCoordinate, endCoordByLevel);
            double loadLength = loadEndCoord - loadStartCoord;
            TraceLogger?.AddMessage($"Total length L,tot = {loadEndCoord}(m) - {loadStartCoord}(m) = {loadLength}(m)");
            double totalLoad = distributedLoad.LoadValue * distributedLoad.LoadRatio * loadLength;
            TraceLogger?.AddMessage($"Total load Q,tot = {distributedLoad.LoadValue}(N/m) * {distributedLoad.LoadRatio} * {loadLength}(m) = {totalLoad}(N)");
            return totalLoad;
        }

        private double GetCoordinateByLevel(double startCoord, double endCoord, double relativeLevel)
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
