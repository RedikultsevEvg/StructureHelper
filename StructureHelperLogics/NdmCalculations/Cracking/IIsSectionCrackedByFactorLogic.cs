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
    /// Check crack appearence beetwen start force tuple and end force tuple
    /// </summary>
    public interface IIsSectionCrackedByFactorLogic : ILogic
    {
        IIsSectionCrackedByForceLogic IsSectionCrackedByForceLogic { get; set; }
        /// <summary>
        /// Start force tupple
        /// </summary>
        IForceTuple StartTuple { get; set; }
        /// <summary>
        /// End force tuple
        /// </summary>
        IForceTuple EndTuple { get; set; }

        /// <summary>
        /// Returns result of checking of crack appearence
        /// </summary>
        /// <param name="factor">factor beetwen start and end tuple, 0 is start tuple, 1 is end tuple</param>
        /// <returns>true when section is cracked</returns>
        bool IsSectionCracked(double factor);
    }
}
