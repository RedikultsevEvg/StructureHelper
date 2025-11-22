using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    /// <summary>
    /// Implements logic of meshing of StructureHelper NdmPrimitives to collection of Loader Indem parts
    /// </summary>
    public interface ITriangulatePrimitiveLogic : ILogic
    {
        /// <summary>
        /// Collection of primitives which have to be meshed
        /// </summary>
        IEnumerable<INdmPrimitive> Primitives { get; set; }
        /// <summary>
        /// Limit state for meshing
        /// </summary>
        LimitStates LimitState { get; set; }
        /// <summary>
        /// Term (duration) of calculating for meshing
        /// </summary>
        CalcTerms CalcTerm { get; set; }
        /// <summary>
        /// Meshes collection of primitives
        /// </summary>
        /// <returns>List of Loader Ndm parts</returns>
        List<INdm> GetNdms();
    }
}