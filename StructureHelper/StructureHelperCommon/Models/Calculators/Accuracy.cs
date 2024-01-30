using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public class Accuracy : IAccuracy
    {
        public double IterationAccuracy { get; set; }
        public int MaxIterationCount { get; set; }
    }
}
