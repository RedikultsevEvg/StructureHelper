using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <inheritdoc/>
    public class ForceTupleInputData : IForceTupleInputData
    {
        /// <inheritdoc/>
        public IEnumerable<INdm> NdmCollection { get; set; }
        /// <inheritdoc/>
        public IForceTuple ForceTuple { get; set; }
        /// <inheritdoc/>
        public IAccuracy Accuracy { get; set; }
        public bool CheckStrainLimit { get; set; }

        public ForceTupleInputData()
        {
            Accuracy ??= new Accuracy() { IterationAccuracy = 0.01d, MaxIterationCount = 1000 };
        }
    }
}
