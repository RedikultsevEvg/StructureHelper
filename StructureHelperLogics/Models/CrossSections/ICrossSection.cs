using StructureHelperCommon.Models.WorkPlanes;

namespace StructureHelperLogics.Models.CrossSections
{
    public interface ICrossSection : ISaveable, ISelfCloneStrategy<ICrossSection>
    {
        ICrossSectionRepository SectionRepository { get; set; }
        IWorkPlaneProperty WorkPlaneProperty { get; set; }
    }
}
