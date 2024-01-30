using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    /// <inheritdoc/>
    internal class ConcreteBucklingOptions : IConcreteBucklingOptions
    {
        /// <inheritdoc/>
        public IForceTuple LongTermTuple { get; set; }
        /// <inheritdoc/>
        public ICompressedMember CompressedMember { get; set; }
        /// <inheritdoc/>
        public LimitStates LimitState { get; set; }
        /// <inheritdoc/>
        public CalcTerms CalcTerm { get; set; }
        /// <inheritdoc/>
        public IEnumerable<INdmPrimitive> Primitives  { get; set; }
        /// <inheritdoc/>
        public IForceTuple CalcForceTuple { get; set; }
}
}
