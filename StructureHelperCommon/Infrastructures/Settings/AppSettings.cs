using System.Text.Json.Serialization;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public class AppSettings
    {
        [JsonPropertyName("RecentFileSettings")]
        public RecentFilesSettings RecentFilesSettings { get; set; } = new();
    }
}
