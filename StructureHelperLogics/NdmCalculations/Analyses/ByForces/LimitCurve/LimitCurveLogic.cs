using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories;
using System.Windows.Media.Media3D;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveLogic : ILimitCurveLogic
    {
        private FindParameterResult result;
        private List<IPoint2D> resultList;
        private int pointCount;
        public ILimitCurveParameterLogic ParameterLogic { get; set; }
        public IGetPredicateLogic GetPredicateLogic { get; private set; }

        private object lockObject = new object();
        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public LimitCurveLogic(ILimitCurveParameterLogic parameterLogic)
        {
            ParameterLogic = parameterLogic;
        }
        public LimitCurveLogic(IGetPredicateLogic getPredicateLogic)
        {
            ParameterLogic = new LimitCurveParameterLogic();
            GetPredicateLogic = getPredicateLogic;
        }
        /// <inheritdoc/>
        public List<IPoint2D> GetPoints(IEnumerable<IPoint2D> points)
        {
            if (TraceLogger is not null)
            {
                ParameterLogic.TraceLogger = TraceLogger;
            }
            result = new();
            resultList = new();
            TraceLogger?.AddMessage($"Predicate name is {GetPredicateLogic.Name}");
            Predicate<IPoint2D> limitPredicate = GetPredicateLogic.GetPredicate();
            //if predicate is true for point (0,0), then check other point is pointless
            var zeroPoint = new Point2D();
            if (limitPredicate(zeroPoint) == true)
            {
                TraceLogger?.AddMessage($"Predicate is true for point. All point will be skiped", TraceLogStatuses.Warning);
                TraceLogger?.AddEntry(new TraceTablesFactory(TraceLogStatuses.Warning).GetByPoint2D(zeroPoint));
                TraceLogger?.AddMessage($"All point will be skiped", TraceLogStatuses.Warning);
                var range = points.Select(point => new Point2D { X = 0d, Y = 0d }).ToList();
                resultList.AddRange(range);
                return resultList;
            }
            pointCount = 0;
            MultiThreadProc(points);
            return resultList;
        }

        private void MultiThreadProc(IEnumerable<IPoint2D> points)
        {
            Task<IPoint2D>[] tasks = new Task<IPoint2D>[points.Count()];
            for (int i = 0; i < points.Count(); i++)
            {
                var point = points.ToList()[i];
                tasks[i] = new Task<IPoint2D>(() => FindResultPoint(point));
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            for (int j = 0; j < points.Count(); j++)
            {
                var taskResult = tasks[j].Result;
                resultList.Add(taskResult);
                ActionToOutputResults?.Invoke(result);
            }
        }

        private Point2D FindResultPoint(IPoint2D point)
        {
            var resultPoint = FindResultPointByPredicate(point);
            lock (lockObject)
            {
                pointCount++;
            }
            result.IterationNumber = pointCount;
            ActionToOutputResults?.Invoke(result);
            return resultPoint;
        }

        private Point2D FindResultPointByPredicate(IPoint2D point)
        {
            ShiftTraceLogger newLogger;
            Predicate<IPoint2D> limitPredicate;

            lock (lockObject)
            {
                newLogger = new ShiftTraceLogger()
                {
                    ShiftPriority = 100
                };
                GetPredicateLogic.TraceLogger = newLogger;
                limitPredicate = GetPredicateLogic.GetPredicate();
            }
            var localCurrentPoint = point.Clone() as IPoint2D;
            var logic = ParameterLogic.Clone() as ILimitCurveParameterLogic;
            logic.TraceLogger = newLogger;
            logic.CurrentPoint = localCurrentPoint;
            logic.LimitPredicate = limitPredicate;
            double parameter;

            if (limitPredicate(localCurrentPoint) == false)
            {
                parameter = 1d;
            }
            else
            {
                parameter = logic.GetParameter();
            }

            var resultPoint = new Point2D()
            {
                X = localCurrentPoint.X * parameter,
                Y = localCurrentPoint.Y * parameter
            };
            lock (lockObject)
            {
                TraceLogger?.AddMessage($"Source point");
                TraceLogger?.AddEntry(new TraceTablesFactory(TraceLogStatuses.Info).GetByPoint2D(localCurrentPoint));
                TraceLogger?.TraceLoggerEntries.AddRange(newLogger.TraceLoggerEntries);
                TraceLogger?.AddMessage($"Parameter value {parameter} was obtained");
                TraceLogger?.AddMessage($"Calculated point\n(X={localCurrentPoint.X} * {parameter} = {resultPoint.X},\nY={localCurrentPoint.Y} * {parameter} = {resultPoint.Y})");
                TraceLogger?.AddEntry(new TraceTablesFactory(TraceLogStatuses.Info).GetByPoint2D(resultPoint));
            }
            return resultPoint;
        }
    }
}
