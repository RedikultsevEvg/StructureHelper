using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic for checking collection of ndms for appearance of crack
    /// </summary>
    public interface ISectionCrackedLogic : ILogic
    {
        /// <summary>
        /// Force tuple for checking of cracks appearence
        /// </summary>
        IForceTuple Tuple { get; set; }
        /// <summary>
        /// Collection of ndms which is checking fo cracking
        /// </summary>
        IEnumerable<INdm> CheckedNdmCollection { get; set; }
        /// <summary>
        /// Full ndms collection of cross-section
        /// </summary>
        IEnumerable<INdm> SectionNdmCollection { get; set; }
        /// <summary>
        /// Returns result of checking of cracks appearence
        /// </summary>
        /// <returns>True if Checked collectition contains cracked elements</returns>
        bool IsSectionCracked();
    }
}
