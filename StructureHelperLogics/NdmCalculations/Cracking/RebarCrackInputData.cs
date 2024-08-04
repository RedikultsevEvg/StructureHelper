using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCrackInputData : IInputData
    {
        /// <summary>
        /// Collection of ndms where work of crackable material in tension was assigned according to material properties
        /// </summary>
        public IEnumerable<INdm> CrackableNdmCollection { get; set; }
        /// <summary>
        /// Collection of ndms where work of concrete is disabled
        /// </summary>
        public IEnumerable<INdm> CrackedNdmCollection { get; set; }
        /// <summary>
        /// Force tuple for calculation
        /// </summary>
        public ForceTuple ForceTuple { get; set; }
        /// <summary>
        /// Base length beetwen cracks
        /// </summary>
        public double LengthBeetwenCracks { get; set; }
    }
}
