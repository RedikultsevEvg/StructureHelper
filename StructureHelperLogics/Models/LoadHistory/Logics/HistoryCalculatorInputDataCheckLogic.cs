using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class HistoryCalculatorInputDataCheckLogic : CheckEntityLogic<IHistoryCalculatorInputData>
    {
        private ICheckEntityLogic<IConstructionStageInputData> stageCheckLogic;
        private ICheckEntityLogic<IConstructionStageInputData> StageCheckLogic => stageCheckLogic ??= new ConstructionStageInputDataCheckLogic(TraceLogger);
        private bool result;

        public HistoryCalculatorInputDataCheckLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public override bool Check()
        {
            result = true;
            if (Entity is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Load history calculator input data";
                TraceMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            if (Entity.ConstructionStages.Count == 0)
            {
                TraceMessage($"History calculator {Entity.HistoryCalculator.Name} doesn't have any construction stages");
                return false;
            }
            foreach (var stage in Entity.ConstructionStages)
            {
                StageCheckLogic.Entity = stage.InputData;
                
                if (StageCheckLogic.Check() == false)
                {
                    TraceMessage(stageCheckLogic.CheckResult);
                    result = false;
                }
            }
            return result;
        }
    }
}
