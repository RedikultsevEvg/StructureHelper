using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class ColumnFilePropertyDTO : IColumnFileProperty
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("SearchingName")]
        public string SearchingName { get; set; }
        [JsonProperty("Index")]
        public int Index { get; set; }
        [JsonProperty("Factor")]
        public double Factor { get; set; }


        public ColumnFilePropertyDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
