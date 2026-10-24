using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IHistoryCalculatorInputData : ISaveable, IInputData
    {
        IHistoryCalculator HistoryCalculator { get; }
        List<IConstructionStage> ConstructionStages { get; }
        IAccuracy Accuracy { get; set; }
        bool CheckStainLimit { get; set; }
    }
}
