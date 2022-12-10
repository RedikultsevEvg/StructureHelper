using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public interface INdmCalculator
    {
        string Name { get; set; }
        /// <summary>
        /// Method for calculating
        /// </summary>
        void Run();
        /// <summary>
        /// Result of Calculations
        /// </summary>
        INdmResult Result { get; }
    }
}
