using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <inheritdoc/>
    public class DivisionSize : IDivisionSize
    {
        /// <inheritdoc/>
        public double NdmMaxSize { get; set; } = 0.01d;
        /// <inheritdoc/>
        public int NdmMinDivision { get; set; } = 10;
        /// <inheritdoc/>
        public bool ClearUnderlying { get; set; } = false;
    }
}
