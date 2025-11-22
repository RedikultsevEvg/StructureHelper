using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Implements input data for search of forces of crack appearance in some range of load
    /// </summary>
    public interface ICrackForceCalculatorInputData: IInputData
    {
        /// <summary>
        /// Start tuple of range for search of forces of crack appearance
        /// </summary>
        IForceTuple StartTuple { get; set; }
        /// <summary>
        /// End tuple of range for search of forces of crack appearance
        /// </summary>
        IForceTuple EndTuple { get; set; }
        /// <summary>
        /// Collection of NdmElements for checking of crack appearance
        /// </summary>
        IEnumerable<INdm> CheckedNdmCollection { get; set; }
        /// <summary>
        /// Collection of initial NdmElements of cross-section 
        /// </summary>
        IEnumerable<INdm> SectionNdmCollection { get; set; }
    }
}
