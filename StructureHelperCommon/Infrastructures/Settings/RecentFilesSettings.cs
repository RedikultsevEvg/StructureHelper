using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public class RecentFilesSettings
    {
        [JsonPropertyName("MaxCount")]
        public int MaxCount { get; set; } = 30;
        [JsonPropertyName("Files")]
        public List<RecentFileSettings> Files { get; set; } = []; 
    }
}
