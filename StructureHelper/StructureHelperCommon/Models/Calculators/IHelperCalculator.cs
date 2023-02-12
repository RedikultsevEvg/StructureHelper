using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public interface IHelperCalculator <in TInputData, TCalculationResult>
        where TInputData : class
        where TCalculationResult : class
    {
    }
}
