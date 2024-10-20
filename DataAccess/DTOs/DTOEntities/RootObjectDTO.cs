using Newtonsoft.Json;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class RootObjectDTO : IRootObjectDTO
    {
        [JsonProperty("FileVersion")]
        public FileVersionDTO FileVersion { get; set; }
        [JsonProperty("Project")]
        public ProjectDTO Project { get; set; }

    }
}
