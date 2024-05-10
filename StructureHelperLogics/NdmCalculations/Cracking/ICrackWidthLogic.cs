using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic for calculating width of crack
    /// </summary>
    public interface ICrackWidthLogic : ILogic
    {
        ICrackWidthLogicInputData InputData { get; set; }
        /// <summary>
        /// return width of crack in meters
        /// </summary>
        double GetCrackWidth();
    }
}
