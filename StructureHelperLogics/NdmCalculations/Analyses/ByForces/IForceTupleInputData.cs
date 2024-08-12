using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Input data for Force Tuple Calculator
    /// </summary>
    public interface IForceTupleInputData : IInputData
    {
        /// <summary>
        /// Collection of ndma-parts for calculation
        /// </summary>
        IEnumerable<INdm> NdmCollection { get; set; }
        /// <summary>
        /// Force tuple which is used for calculation
        /// </summary>
        IForceTuple ForceTuple { get; set; }
        /// <summary>
        /// Settings of iteration
        /// </summary>
        IAccuracy Accuracy { get; set; }
    }
}
