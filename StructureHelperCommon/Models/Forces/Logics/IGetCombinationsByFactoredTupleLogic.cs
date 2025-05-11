namespace StructureHelperCommon.Models.Forces.Logics
{
    /// <summary>
    /// Returns combinations for source combination and properties
    /// </summary>
    public interface IGetCombinationsByFactoredTupleLogic
    {        /// <summary>
        /// Source combination of forces
        /// </summary>
        IForceTuple? SourceForceTuple { get; set; }
        /// <summary>
        /// Properties of combination for source combination of force
        /// </summary>
        IFactoredCombinationProperty? CombinationProperty { get; set; }

        /// <inheritdoc/>
        IForceActionProperty? ForceActionProperty { get; set; }
        /// <summary>
        /// Returns combination list
        /// </summary>
        /// <returns></returns>
        IForceCombinationList GetCombinationList();
    }
}