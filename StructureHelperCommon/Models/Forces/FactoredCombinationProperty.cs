using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class FactoredCombinationProperty : IFactoredCombinationProperty
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public CalcTerms CalcTerm { get; set; } = CalcTerms.ShortTerm;
        /// <inheritdoc/>
        public LimitStates LimitState { get; set; } = LimitStates.SLS;
        /// <inheritdoc/>
        public double LongTermFactor { get; set; } = 1d;
        /// <inheritdoc/>
        public double ULSFactor { get; set; } = 1.2d;
        
        public FactoredCombinationProperty(Guid id)
        {
            Id = id;
        }
        public FactoredCombinationProperty() : this(Guid.NewGuid())
        {
            
        }
    }
}
