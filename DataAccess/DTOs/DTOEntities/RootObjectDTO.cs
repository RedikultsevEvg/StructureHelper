using Newtonsoft.Json;

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
