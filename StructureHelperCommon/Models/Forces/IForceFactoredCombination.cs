using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Supports common properties of factored combination of forces
    /// </summary>
    public interface IForceFactoredCombination : IForceAction
    {
        /// <summary>
        /// Properties of factored combination of forces
        /// </summary>
        IFactoredCombinationProperty CombinationProperty { get; }
    }
}