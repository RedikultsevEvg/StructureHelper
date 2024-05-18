using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Interface of logic of obtaining of length between cracks
    /// </summary>
    public interface ILengthBetweenCracksLogic : ILogic
    {
        /// <summary>
        /// Full collection of ndm parts of cross-section
        /// </summary>
        IEnumerable<INdm> NdmCollection { get; set; }
        /// <summary>
        /// Strain matrix in cracked state
        /// </summary>
        IStrainMatrix StrainMatrix { get; set; }
        /// <summary>
        /// Returns length between cracks
        /// </summary>
        /// <returns>Length betwenn cracks</returns>
        double GetLength();
    }
}
