using Newtonsoft.Json;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{

    [JsonObject(IsReference = true)]
    public class FileVersionDTO : IFileVersion
    {

        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("VersionNumber")]
        public int VersionNumber { get; set; }
        [JsonProperty("SubVersionNumber")]
        public int SubVersionNumber { get; set; }
        public FileVersionDTO(Guid id)
        {
            Id = id;
        }
        public FileVersionDTO() : this (Guid.NewGuid())
        {
            
        }
    }
}
