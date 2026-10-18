using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class HistoryCalculatorResult : IHistoryCalculatorResult
    {
        public IHistoryCalculator HistoryCalculator { get; }
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
        public List<ISubStageResult> StagesResults { get; } = [];

        public HistoryCalculatorResult(IHistoryCalculator historyCalculator)
        {
            HistoryCalculator = historyCalculator;
        }
    }
}
