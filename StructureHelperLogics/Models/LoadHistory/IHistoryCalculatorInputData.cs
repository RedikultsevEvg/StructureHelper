using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IHistoryCalculatorInputData
    {
        IAccuracy Accuracy { get; set; }
        bool CheckStainLimit { get; set; }
    }
}
