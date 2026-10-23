using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    internal class HistoryCalculatorInputData : IHistoryCalculatorInputData
    {
        public IAccuracy Accuracy { get; set; } = new Accuracy() { IterationAccuracy = 0.001, MaxIterationCount = 1000};
        public bool CheckStainLimit { get; set; } = true;
    }
}
