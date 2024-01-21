using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveParameterLogic : ILimitCurveParameterLogic
    {
        private FindParameterResult result;
        public Predicate<Point2D> LimitPredicate { get; set; }
        public IPoint2D CurrentPoint { get; set; }
        public Action<IResult> ActionToOutputResults { get; set; }
        public ITraceLogger? TraceLogger { get; set; }

        public LimitCurveParameterLogic()
        {
            
        }

        public double GetParameter()
        {
            var parameterCalculator = new FindParameterCalculator()
            {
                Predicate = GetFactorPredicate,
            };
            if (TraceLogger is not null) { parameterCalculator.TraceLogger = TraceLogger; }
            parameterCalculator.Accuracy.IterationAccuracy = 0.001d;
            parameterCalculator.Run();
            if (parameterCalculator.Result.IsValid == false)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": predicate for point  (x={CurrentPoint.X}, y={CurrentPoint.Y}) is not valid");
            }
            result = parameterCalculator.Result as FindParameterResult;
            var parameter = result.Parameter;
            if (parameter < 0.1d)
            {
                parameterCalculator.Accuracy.IterationAccuracy = 0.0001d;
                parameterCalculator.Run();
                result = parameterCalculator.Result as FindParameterResult;
                parameter = result.Parameter;
            }

            return parameter;
        }

        private bool GetFactorPredicate(double factor)
        {
            var newPoint = new Point2D() { X = CurrentPoint.X * factor, Y = CurrentPoint.Y * factor };
            return LimitPredicate(newPoint);
        }

        public object Clone()
        {
            var newItem = new LimitCurveParameterLogic();
            newItem.LimitPredicate = LimitPredicate;
            newItem.CurrentPoint = (CurrentPoint ?? new Point2D()).Clone() as IPoint2D;
            newItem.ActionToOutputResults = ActionToOutputResults;
            return newItem;
        }
    }
}
