using Newtonsoft.Json;
using StructureHelperCommon.Models.WorkPlanes;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrossSectionDTO : ICrossSection
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("SectionRepository")]
        public ICrossSectionRepository SectionRepository { get; set; }

        public IWorkPlaneProperty WorkPlaneProperty { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
