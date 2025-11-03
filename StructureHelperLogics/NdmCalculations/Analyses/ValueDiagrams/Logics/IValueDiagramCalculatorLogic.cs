
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public interface IValueDiagramCalculatorLogic : ILogic
    {
        IValueDiagramCalculatorInputData InputData { get; set; }
        IValueDiagramCalculatorResult GetResult();
    }
}
