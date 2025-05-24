using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IBeamShearCalculator : ICalculator
    {
        IBeamShearCalculatorInputData InputData { get; set; }
        bool ShowTraceData { get; set; }
    }
}
