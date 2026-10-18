using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IHistoryCalculatorResult : IResult
    {
        IHistoryCalculator HistoryCalculator { get; }
        List<ISubStageResult> StagesResults { get; }
    }
}
