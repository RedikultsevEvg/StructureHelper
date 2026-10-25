using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class HistoryCalculatorResult : IHistoryCalculatorResult
    {
        public IHistoryCalculatorInputData InputData { get; }
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public List<IStageResult> StagesResults { get; } = [];

        public HistoryCalculatorResult()
        {
            
        }
        public HistoryCalculatorResult(IHistoryCalculatorInputData inputData)
        {
            InputData = inputData;
        }
    }
}
