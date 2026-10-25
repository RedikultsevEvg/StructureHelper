using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IHistoryCalculatorResult : IResult
    {
        IHistoryCalculatorInputData InputData { get; }
        List<IStageResult> StagesResults { get; }
    }
}
