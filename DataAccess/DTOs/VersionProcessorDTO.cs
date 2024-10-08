using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class VersionProcessorDTO : IVersionProcessor
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Versions")]
        public List<IDateVersion> Versions { get; } = new();

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
