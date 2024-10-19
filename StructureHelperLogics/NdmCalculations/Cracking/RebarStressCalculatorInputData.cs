using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class RebarStressCalculatorInputData : IRebarStressCalculatorInputData
    {
        /// <inheritdoc/>
        public ForceTuple ForceTuple { get; set; }
        /// <inheritdoc/>
        public IEnumerable<INdm> NdmCollection { get; set; }
        /// <inheritdoc/>
        public IRebarNdmPrimitive RebarPrimitive { get; set; }
    }
}
