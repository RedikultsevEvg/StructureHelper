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
                //throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": predicate for zero value is not valid");
            }
            foreach (var point in points)
            {
                FindParameter(point);
            }
            return resultList;
        }

        private void FindParameter(IPoint2D point)
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
                parameter = parameterLogic.GetParameter();
            }
            var resultPoint = new Point2D()
            {
                X = currentPoint.X * parameter,
                Y = currentPoint.Y * parameter
            };
            resultList.Add(resultPoint);
            result.IterationNumber = resultList.Count;
            ActionToOutputResults?.Invoke(result);
        }
    }
}
