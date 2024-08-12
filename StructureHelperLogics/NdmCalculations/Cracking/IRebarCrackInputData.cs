using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Input data for calculating of width of crack by ndm collection
    /// </summary>
    public interface IRebarCrackInputData : IInputData
    {
        /// <summary>
        /// Collection of ndms where work of crackable material in tension was assigned according to material properties
        /// </summary>
        IEnumerable<INdm> CrackableNdmCollection { get; set; }
        /// <summary>
        /// Collection of ndms where work of concrete is disabled
        /// </summary>
        IEnumerable<INdm> CrackedNdmCollection { get; set; }
        /// <summary>
        /// Force tuple for calculation
        /// </summary>
        ForceTuple ForceTuple { get; set; }
        /// <summary>
        /// Base length beetwen cracks
        /// </summary>
        double LengthBeetwenCracks { get; set; }
    }
}