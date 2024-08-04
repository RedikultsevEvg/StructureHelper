using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class CrackedConcreteNdmLogic : IIsSectionCrackedByForceLogic
    {
        /// <inheritdoc/>
        public INdm ConcreteNdm { get; set; }
        /// <inheritdoc/>
        public IForceTuple Tuple { get; set; }
        /// <inheritdoc/>
        public IEnumerable<INdm> CheckedNdmCollection { get; set; }
        /// <inheritdoc/>
        public IEnumerable<INdm> SectionNdmCollection { get; set; }
        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }

        /// <inheritdoc/>
        public bool IsSectionCracked()
        {
            throw new NotImplementedException();
        }
    }
}
