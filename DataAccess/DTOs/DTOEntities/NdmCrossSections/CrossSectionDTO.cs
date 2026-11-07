using Newtonsoft.Json;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperLogics.Models.CrossSections;

namespace DataAccess.DTOs
{
    public class CrossSectionDTO : ICrossSection
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("SectionRepository")]
        public ICrossSectionRepository SectionRepository { get; set; }

        public IWorkPlaneProperty WorkPlaneProperty { get; set; }

        public CrossSectionDTO()
            : this(Guid.NewGuid())
        {
        }

        public CrossSectionDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
