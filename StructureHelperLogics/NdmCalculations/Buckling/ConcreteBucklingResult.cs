using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    /// <inheritdoc/>
    public class ConcreteBucklingResult : IConcreteBucklingResult
    {
        /// <inheritdoc/>
        public bool IsValid { get; set; }
        /// <inheritdoc/>
        public string Description { get; set; }
        /// <inheritdoc/>
        public double EtaFactorAlongX { get; set; }
        /// <inheritdoc/>
        public double EtaFactorAlongY { get; set; }
    }
}
