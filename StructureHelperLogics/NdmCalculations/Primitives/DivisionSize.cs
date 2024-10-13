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
        public Guid Id { get; }
        /// <inheritdoc/>
        public double NdmMaxSize { get; set; } = 0.01d;
        /// <inheritdoc/>
        public int NdmMinDivision { get; set; } = 10;
        /// <inheritdoc/>
        public bool ClearUnderlying { get; set; } = false;

        public DivisionSize(Guid id)
        {
            Id = id;
        }

        public DivisionSize() : this (Guid.NewGuid())
        {
            
        }
    }
}
