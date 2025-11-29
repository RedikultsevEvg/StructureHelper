using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class CrackForceCalculatorInputData : ICrackForceCalculatorInputData
    {
        /// <inheritdoc/>
        public IForceTuple StartTuple { get; set; } = new ForceTuple();
        /// <inheritdoc/>
        public IForceTuple EndTuple { get; set; } = new ForceTuple();
        /// <inheritdoc/>
        public IEnumerable<INdm> CheckedNdmCollection { get; set; }
        /// <inheritdoc/>
        public IEnumerable<INdm> SectionNdmCollection { get; set; }
    }
}
