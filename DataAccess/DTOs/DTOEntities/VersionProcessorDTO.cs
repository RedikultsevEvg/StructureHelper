using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;

namespace DataAccess.DTOs
{
    public class VersionProcessorDTO : IVersionProcessor
    {
        [JsonProperty("Id")]
        public Guid Id { get;}
        [JsonProperty("Versions")]
        public List<IDateVersion> Versions { get; } = new();

        public VersionProcessorDTO(Guid id)
        {
            Id = id;
        }

        public void AddVersion(ISaveable newItem)
        {
            throw new NotImplementedException();
        }

        public IDateVersion GetCurrentVersion()
        {
            throw new NotImplementedException();
        }
    }
}
