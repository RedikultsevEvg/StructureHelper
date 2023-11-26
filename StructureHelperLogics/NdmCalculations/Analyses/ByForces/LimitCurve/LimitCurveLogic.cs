using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveLogic : ILimitCurveLogic
    {
        private IPoint2D currentPoint;
        private ILimitCurveParameterLogic parameterLogic;
        public Predicate<IPoint2D> LimitPredicate { get; set; }
        public LimitCurveLogic(ILimitCurveParameterLogic parameterLogic)
        {
            this.parameterLogic = parameterLogic;
        }
        public LimitCurveLogic(Predicate<IPoint2D> limitPredicate) : this (new LimitCurveParameterLogic(limitPredicate))
        {
            LimitPredicate = limitPredicate;
        }
        public List<IPoint2D> GetPoints(List<IPoint2D> points)
        {
            List<IPoint2D> resultList = new();
            if (LimitPredicate(new Point2D()) == true)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": predicate for zero value is not valid");
            }
            foreach (var point in points)
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
            }
            return resultList;
        }
    }
}
