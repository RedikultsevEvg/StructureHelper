using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;


namespace StructureHelperCommon.Infrastructures.Settings
{
    public class RecentFileSettings
    {
        [JsonPropertyName("FileName")]
        public string FileName { get; set; } = string.Empty;
        [JsonPropertyName("RecentOpeningTime")]
        public DateTime RecentOpeningTime { get; set; } = DateTime.Now;
    }
}
