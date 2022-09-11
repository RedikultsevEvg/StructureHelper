using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Calculations.CalculationProperties
{
    public class IterationProperty : IIterationProperty
    {
        public double Accuracy { get; set; }
        public int MaxIterationCount { get; set; }
    }
}
