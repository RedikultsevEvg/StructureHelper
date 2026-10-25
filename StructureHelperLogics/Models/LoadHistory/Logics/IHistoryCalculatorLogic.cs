using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory.Logics
{
    internal interface IHistoryCalculatorLogic : ILogic
    {
        IHistoryCalculatorResult CalculateResult(IHistoryCalculatorInputData data);
    }
}
