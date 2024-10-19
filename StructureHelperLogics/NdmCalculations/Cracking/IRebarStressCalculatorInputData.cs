using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Input data for rebar stress calculator
    /// </summary>
    public interface IRebarStressCalculatorInputData : IInputData
    {
        /// <summary>
        /// Force tuple for calculting
        /// </summary>
        ForceTuple ForceTuple { get; set; }
        /// <summary>
        /// Collection of ndms of cross-section (as usual without concrete tensile strength)
        /// </summary>
        IEnumerable<INdm> NdmCollection { get; set; }
        /// <summary>
        /// Rebar which stress and strain will be obtained for 
        /// </summary>
        IRebarNdmPrimitive RebarPrimitive { get; set; }
    }
}
