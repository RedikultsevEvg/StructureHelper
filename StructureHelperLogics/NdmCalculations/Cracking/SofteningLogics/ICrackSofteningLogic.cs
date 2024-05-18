using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic of calculating of factor of softening in crack of RC structures
    /// </summary>
    public interface ICrackSofteningLogic : ILogic
    {
        /// <summary>
        /// Returns softening factor
        /// </summary>
        /// <returns></returns>
        double GetSofteningFactor();
    }
}
