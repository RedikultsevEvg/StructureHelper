using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackWidthLogicInputData : IInputData
    {
        /// <summary>
        /// strain of rebar, dimensionless
        /// </summary>
        double RebarStrain { get; set; }
        /// <summary>
        /// strain of concrete, dimensionless
        /// </summary>
        double ConcreteStrain { get; set; }
        /// <summary>
        /// Length between cracks in meters
        /// </summary>
        double LengthBetweenCracks { get; set; }
    }
}
