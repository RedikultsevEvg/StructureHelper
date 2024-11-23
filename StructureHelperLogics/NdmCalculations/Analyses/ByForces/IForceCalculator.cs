using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Provides calculations of ndm primitives for force actions
    /// </summary>
    public interface IForceCalculator : ICalculator, IHasActionByResult
    {
        IForceCalculatorInputData InputData { get; set; }
    }
}
