using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements logic for obtaining of inclined section
    /// </summary>
    public interface IGetInclinedSectionLogic : ILogic
    {
        /// <summary>
        /// Returns inclined section
        /// </summary>
        /// <returns>Inclined section</returns>
        IInclinedSection GetInclinedSection();
    }
}
