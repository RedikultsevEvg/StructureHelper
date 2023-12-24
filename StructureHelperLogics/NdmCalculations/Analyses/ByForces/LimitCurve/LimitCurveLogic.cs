using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories;

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
            result = new();
            resultList = new();
            Predicate<IPoint2D> limitPredicate = GetPredicateLogic.GetPredicate();
            if (limitPredicate(new Point2D()) == true)
            {
                var range = points.Select(point => new Point2D { X = point.X * 0d, Y = point.Y * 0d }).ToList();
                resultList.AddRange(range);
                return resultList;
            }
            pointCount = 0;
            MultyThreadProc(points);
            //MonoThreadProc(points);
            return resultList;
        }

        private void MultyThreadProc(IEnumerable<IPoint2D> points)
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

        private void MonoThreadProc(IEnumerable<IPoint2D> points)
        {
            foreach (var point in points)
            {
                FindParameter(point);
            }
        }

        private void FindParameter(IPoint2D point)
        {
            IPoint2D resultPoint = FindResultPoint(point);
            resultList.Add(resultPoint);
            lock (lockObject)
            {
                pointCount++;
            }
            result.IterationNumber = pointCount;
            ActionToOutputResults?.Invoke(result);
        }

        private Point2D FindResultPoint(IPoint2D point)
        {
            double parameter;
            var locCurrentPoint = point.Clone() as IPoint2D;
            Predicate<IPoint2D> limitPredicate;
            lock (lockObject)
            {
                limitPredicate = GetPredicateLogic.GetPredicate();
            }
            var logic = ParameterLogic.Clone() as ILimitCurveParameterLogic;
            logic.CurrentPoint = locCurrentPoint;
            logic.LimitPredicate = limitPredicate;

            if (limitPredicate(locCurrentPoint) == false)
            {
                parameter = 1d;
            }
            else
            {
                parameter = logic.GetParameter();
            }
            
            var resultPoint = new Point2D()
            {
                X = locCurrentPoint.X * parameter,
                Y = locCurrentPoint.Y * parameter
            };
            lock (lockObject)
            {
                pointCount++;
            }
            result.IterationNumber = pointCount;
            ActionToOutputResults?.Invoke(result);
            return resultPoint;
            //}
        }
    }
}
