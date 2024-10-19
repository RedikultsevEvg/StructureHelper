using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceCalculatorLogic : ILogic, IHasActionByResult
    {
        IForceCalculatorInputData InputData { get; set; }
        ForcesResults GetForcesResults();
    }
}
