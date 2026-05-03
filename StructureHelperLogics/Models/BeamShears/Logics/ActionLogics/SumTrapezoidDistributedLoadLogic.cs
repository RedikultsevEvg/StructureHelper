using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.BeamShears.Logics.ActionLogics
{
    public class SumTrapezoidDistributedLoadLogic : ISumForceByShearLoadLogic
    {
        private ICoordinateByLevelLogic coordinateByLevelLogic;
        private ICoordinateByLevelLogic CoordinateByLevelLogic => coordinateByLevelLogic ??= new CoordinateByLevelLogic(TraceLogger);
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private ITrapezoidDistributedLoad trapezoidLoad;
        private double sourceLoadLength;
        private double sectionLoadLength;
        private double loadFactor;
        private double sumFactor;

        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();

        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public SumTrapezoidDistributedLoadLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IForceTuple GetSumShearForce(IBeamSpanLoad beamShearLoad, double startCoord, double endCoord)
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            if (beamShearLoad is not ITrapezoidDistributedLoad distributedLoad)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(beamShearLoad) + ": beam shear load is not trapezoid distributed load");
            }
            this.trapezoidLoad = distributedLoad;
            IForceTuple sumForce = GetTrapezoidLoadSum(startCoord, endCoord);
            return sumForce;
        }

        private IForceTuple GetTrapezoidLoadSum(double startCoord, double endCoord)
        {
            GetLengthOfLoad();
            GetLoadFactor(trapezoidLoad);
            TraceLogger?.AddMessage($"Trapezoid distributed load Name = {trapezoidLoad.Name}, Start value = {trapezoidLoad.StartLoadValue.Qy}(N/m) at point {trapezoidLoad.StartCoordinate}(m), End value = {trapezoidLoad.EndLoadValue.Qy}(N/m) at point {trapezoidLoad.EndCoordinate}(m), total length of load L,tot = {trapezoidLoad.EndCoordinate} - {trapezoidLoad.StartCoordinate} = {sourceLoadLength}(m)");

            double loadStartCoord = Math.Max(trapezoidLoad.StartCoordinate, 0d);
            if (loadStartCoord > endCoord)
            {
                TraceLogger?.AddMessage($"Load start coordinate {loadStartCoord}(m) is bigger than section end {endCoord}(m), so total load is zero");
                return new ForceTuple(Guid.NewGuid());
            }
            double endCoordByLevel = CoordinateByLevelLogic.GetCoordinate(startCoord, endCoord, trapezoidLoad.RelativeLoadLevel);
            double loadEndCoord = Math.Min(trapezoidLoad.EndCoordinate, endCoordByLevel);
            sectionLoadLength = loadEndCoord - loadStartCoord;
            TraceLogger?.AddMessage($"Total design length L,tot = {loadEndCoord}(m) - {loadStartCoord}(m) = {sectionLoadLength}(m)");
            IForceTuple startLoad = GetInterpolatedTuple(loadStartCoord);
            TraceLogger?.AddMessage($"Start load Qy,start = {startLoad.Qy}(N/m) at point {loadStartCoord}(m)");
            IForceTuple endLoad = GetInterpolatedTuple(loadEndCoord);
            TraceLogger?.AddMessage($"Start load Qy,end = {endLoad.Qy}(N/m) at point {loadEndCoord}(m)");
            IForceTuple middleTuple = ForceTupleServiceLogic.SumTuples(startLoad, endLoad);
            middleTuple = ForceTupleServiceLogic.MultiplyTupleByFactor(middleTuple, 0.5);
            sumFactor = trapezoidLoad.LoadRatio * sectionLoadLength * loadFactor;
            IForceTuple totalLoad = ForceTupleServiceLogic.MultiplyTupleByFactor(middleTuple, sumFactor);
            TraceLogger?.AddMessage($"Total load Q,tot = ({startLoad.Qy}(N/m) + {endLoad}(N/m)) / 2 * {sectionLoadLength}(m) * {loadFactor} = {totalLoad.Qy}(N)");
            return totalLoad;
        }

        private IForceTuple GetInterpolatedTuple(double loadStartCoord)
        {
            double startLengthFactor = GetLengthFactor(loadStartCoord);
            IForceTuple startLoad = ForceTupleServiceLogic.InterpolateTuples(trapezoidLoad.StartLoadValue, trapezoidLoad.EndLoadValue, startLengthFactor);
            return startLoad;
        }

        private double GetLengthFactor(double pointCoord)
        {
            double lengthBetweenStartOfLoadAndStartOfsection = pointCoord - trapezoidLoad.StartCoordinate;
            return lengthBetweenStartOfLoadAndStartOfsection / sourceLoadLength;
        }

        private void GetLengthOfLoad()
        {
            sourceLoadLength = trapezoidLoad.EndCoordinate - trapezoidLoad.StartCoordinate;
        }

        private void GetLoadFactor(IBeamSpanLoad spanLoad)
        {
            var getFactorLogic = new GetFactorByFactoredCombinationProperty()
            {
                CombinationProperty = spanLoad.CombinationProperty,
                LimitState = LimitState,
                CalcTerm = CalcTerm
            };
            loadFactor = getFactorLogic.GetFactor();
        }
    }
}
