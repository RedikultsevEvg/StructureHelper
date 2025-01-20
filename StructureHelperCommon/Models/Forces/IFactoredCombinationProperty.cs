using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Properties of factored combination of forces
    /// </summary>
    public interface IFactoredCombinationProperty : ISaveable
    {
        /// <summary>
        /// Term of calculation for assigned combination
        /// </summary>
        CalcTerms CalcTerm { get; set; }
        /// <summary>
        /// Limit state for assigned combination
        /// </summary>
        LimitStates LimitState { get; set; }
        /// <summary>
        /// Factor of converting of short term load to long term load
        /// </summary>
        double LongTermFactor { get; set; }
        /// <summary>
        /// Factor of converting serviceability state load to ultimate limit state load 
        /// </summary>
        double ULSFactor { get; set; }
    }
}
