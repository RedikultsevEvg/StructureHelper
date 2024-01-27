using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveCalculator : ILimitCurveCalculator
    {
        private LimitCurveResult result;
        private List<IPoint2D> surroundList;
        private List<IPoint2D> factoredList;
        private ILimitCurveLogic limitCurveLogic;

        public string Name { get; set; }
        public SurroundData SurroundData { get; set; }
        public int PointCount { get; set; }
        public ISurroundProc SurroundProcLogic { get; set; }

        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public LimitCurveCalculator(ILimitCurveLogic limitCurveLogic)
        {
            this.limitCurveLogic = limitCurveLogic;
            SurroundData = new();
            SurroundProcLogic = new RectSurroundProc();
        }

        public LimitCurveCalculator(ILimitCurveParameterLogic parameterLogic)
            : this(new LimitCurveLogic(parameterLogic))
        {
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            if (TraceLogger is not null) { limitCurveLogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(50); }
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLoggerStatuses.Service);
            TraceLogger?.AddMessage($"Start solution in calculator {Name}");
            result = new LimitCurveResult();
            result.IsValid = true;
            result.Name = Name;
            SurroundProcLogic.SurroundData = SurroundData;
            SurroundProcLogic.PointCount = PointCount;
            TraceLogger?.AddMessage($"Point count {PointCount}");
            surroundList = SurroundProcLogic.GetPoints();
            TraceLogger?.AddMessage($"There are {surroundList.Count()} point prepared for calculation");
            if (TraceLogger is not null)
            {
                AddTAbleToTraceLoggerByPoints(surroundList);
            }
            try
            {
                limitCurveLogic.ActionToOutputResults = GetCurrentStepNumber;
                factoredList = limitCurveLogic.GetPoints(surroundList);
                TraceLogger?.AddMessage($"Solution was successfully obtained for {factoredList.Count()} point");
                if (TraceLogger is not null)
                {
                    AddTAbleToTraceLoggerByPoints(factoredList);
                }
                result.Points = factoredList;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage($"Calculation result is not valid: {ex.Message}", TraceLoggerStatuses.Error);
                result.IsValid = false;
                result.Description += ex.Message;
            }
        }

        private void AddTAbleToTraceLoggerByPoints(IEnumerable<IPoint2D> pointList)
        {
            var table = TraceLoggerTableByPointsFactory.GetTableByPoint2D(pointList);
            table.Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info) + TraceLogger.ShiftPriority;
            TraceLogger.AddEntry(table);
        }

        private void GetCurrentStepNumber(IResult calcResult)
        {
            if (calcResult is not FindParameterResult)
            {
                throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(FindParameterResult), calcResult));
            }
            var parameterResult = calcResult as FindParameterResult;
            result.IterationNumber = parameterResult.IterationNumber;
            ActionToOutputResults?.Invoke(result);
        }
    }
}
