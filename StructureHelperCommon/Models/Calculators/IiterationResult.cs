using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public interface IiterationResult
    {
        int MaxIterationCount { get; set; }
        int IterationNumber { get; set; }
    }
}
