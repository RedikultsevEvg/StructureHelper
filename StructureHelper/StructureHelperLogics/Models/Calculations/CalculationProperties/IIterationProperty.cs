using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Calculations.CalculationProperties
{
    public interface IIterationProperty
    {
        double Accuracy { get; set; }
        int MaxIterationCount { get; set; }
    }
}
