namespace StructureHelperCommon.Models.Forces.Logics
{
    /// <summary>
    /// Return combinations for source combination and properties
    /// </summary>
    public interface IGetCombinationByFactoredTupleLogic
    {
        /// <summary>
        /// Source combination of forces
        /// </summary>
        IForceTuple? SourceForceTuple { get; set; }
        /// <inheritdoc/>
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