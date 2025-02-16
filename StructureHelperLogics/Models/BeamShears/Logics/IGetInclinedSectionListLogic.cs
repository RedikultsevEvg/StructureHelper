using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Returns collection of inclined section for beam shear calculating
    /// </summary>
    public interface IGetInclinedSectionListLogic : ILogic
    {
        List<IInclinedSection> GetInclinedSections();
    }
}
