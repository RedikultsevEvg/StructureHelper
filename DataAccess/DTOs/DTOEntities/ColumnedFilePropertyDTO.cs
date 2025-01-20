using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;

namespace DataAccess.DTOs
{
    public class ColumnedFilePropertyDTO : IColumnedFileProperty
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("FilePath")]
        public string FilePath { get; set; } = string.Empty;
        [JsonProperty("SkipRowBeforeHeaderCount")]
        public int SkipRowBeforeHeaderCount { get; set; }
        [JsonProperty("SkipRowHeaderCount")]
        public int SkipRowHeaderCount { get; set; }
        [JsonProperty("GlobalFactor")]
        public double GlobalFactor { get; set; }
        [JsonProperty("ColumnProperties")]
        public List<IColumnFileProperty> ColumnProperties { get; } = new();


        public ColumnedFilePropertyDTO(Guid id)
        {
            Id = id;
        }


        public object Clone()
        {
            return this;
        }
    }
}
