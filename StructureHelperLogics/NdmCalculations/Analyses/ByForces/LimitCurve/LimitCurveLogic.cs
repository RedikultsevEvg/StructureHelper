using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveLogic : ILimitCurveLogic
    {
        private FindParameterResult result;
        private List<IPoint2D> resultList;
        private IPoint2D currentPoint;
        private ILimitCurveParameterLogic parameterLogic;
        public Predicate<IPoint2D> LimitPredicate { get; set; }
        public Action<IResult> ActionToOutputResults { get; set; }

        public LimitCurveLogic(ILimitCurveParameterLogic parameterLogic)
        {
            this.parameterLogic = parameterLogic;
        }
        public LimitCurveLogic(Predicate<IPoint2D> limitPredicate) : this (new LimitCurveParameterLogic(limitPredicate))
        {
            LimitPredicate = limitPredicate;
        }
        /// <inheritdoc/>
        public List<IPoint2D> GetPoints(IEnumerable<IPoint2D> points)
        {
            result = new();
            resultList = new();
            if (LimitPredicate(new Point2D()) == true)
            {
                var range = points.Select(point => new Point2D { X = point.X * 0d, Y = point.Y * 0d }).ToList();
                resultList.AddRange(range);
                return resultList;
            }

            //MultyProcessPoints(points);
            MonoProcessPoints(points);
            return resultList;
        }

        private void MultyProcessPoints(IEnumerable<IPoint2D> points)
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
                result.IterationNumber = resultList.Count;
                ActionToOutputResults?.Invoke(result);
            }
        }

        private void MonoProcessPoints(IEnumerable<IPoint2D> points)
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
            result.IterationNumber = resultList.Count;
            ActionToOutputResults?.Invoke(result);
        }

        private Point2D FindResultPoint(IPoint2D point)
        {
            double parameter;
            currentPoint = point.Clone() as IPoint2D;
            parameterLogic.CurrentPoint = currentPoint;
            if (LimitPredicate(point) == false)
            {
                parameter = 1d;
            }
            else
            {
                parameter =  parameterLogic.GetParameter();
            }
            var resultPoint = new Point2D()
            {
                X = currentPoint.X * parameter,
                Y = currentPoint.Y * parameter
            };
            return resultPoint;
        }
    }
}
