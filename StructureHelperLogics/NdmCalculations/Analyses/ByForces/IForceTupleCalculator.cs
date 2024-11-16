using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Calculator for obtaining solution from loader calculator 
    /// </summary>
    public interface IForceTupleCalculator : ICalculator, IHasActionByResult
    {
        /// <summary>
        /// Input data for analysis
        /// </summary>
        IForceTupleInputData InputData {get;set;}
    }
}
