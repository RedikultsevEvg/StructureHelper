using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class HistoryCalculatorInputData : IHistoryCalculatorInputData
    {
        public Guid Id { get; }
        public List<IConstructionStage> ConstructionStages { get; } = [];
        public IAccuracy Accuracy { get; set; } = new Accuracy() { IterationAccuracy = 0.001, MaxIterationCount = 1000 };
        public bool CheckStrainLimit { get; set; } = true;

        public IHistoryCalculator HistoryCalculator { get; }

        public HistoryCalculatorInputData(Guid id, IHistoryCalculator historyCalculator)
        {
            Id = id;
            HistoryCalculator = historyCalculator;
        }
    }
}
