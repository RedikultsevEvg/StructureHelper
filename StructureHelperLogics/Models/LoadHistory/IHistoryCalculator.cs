using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IHistoryCalculator : ICalculator
    {
        List<IConstructionStage> ConstructionStages { get; }
        IHistoryCalculatorInputData InputData { get; }
    }
}
