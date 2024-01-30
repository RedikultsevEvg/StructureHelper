using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public interface IAccuracy
    {
        double IterationAccuracy { get; set; }
        int MaxIterationCount { get; set; }
    }
}
