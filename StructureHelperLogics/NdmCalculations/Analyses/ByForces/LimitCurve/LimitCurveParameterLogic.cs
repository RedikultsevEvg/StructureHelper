using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveParameterLogic : ILimitCurveParameterLogic
    {
        private FindParameterResult result;
        public Predicate<Point2D> LimitPredicate { get; set; }
        public IPoint2D CurrentPoint { get; set; }
        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public Guid Id { get; }

        public LimitCurveParameterLogic(Guid id)
        {
            Id = id;
        }

        public LimitCurveParameterLogic() : this(Guid.NewGuid())
        {
            
        }

        public double GetParameter()
        {
            var parameterCalculator = new FindParameterCalculator();
            parameterCalculator.InputData.Predicate = GetFactorPredicate;
            if (TraceLogger is not null)
            {
                parameterCalculator.TraceLogger = TraceLogger;
            }
            parameterCalculator.Accuracy.IterationAccuracy = 0.001d;
            parameterCalculator.Run();
            if (parameterCalculator.Result.IsValid == false)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": predicate for point  (x={CurrentPoint.X}, y={CurrentPoint.Y}) is not valid");
            }
            result = parameterCalculator.Result as FindParameterResult;
            var parameter = result.Parameter;
            var limitparamValue = 0.1d;
            if (parameter < limitparamValue)
            {
                var newAccuracy = limitparamValue / 10d;
                TraceLogger?.AddMessage($"Since current parameter value {parameter} has a low accuracy (value less than {limitparamValue}) new parameter calculating is started", TraceLogStatuses.Warning);
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

        public ILimitCurveParameterLogic GetClone(ILimitCurveParameterLogic sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            var newItem = new LimitCurveParameterLogic(id);
            newItem.LimitPredicate = LimitPredicate;
            IPoint2D point2D = CurrentPoint ?? new Point2D();
            newItem.CurrentPoint = point2D.GetClone(point2D);
            newItem.ActionToOutputResults = ActionToOutputResults;
            return newItem;
        }
    }
}
