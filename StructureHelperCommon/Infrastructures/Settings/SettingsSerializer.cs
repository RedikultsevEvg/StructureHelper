using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public static class SettingsSerializer
    {
        private const string fileName = "Settings.json";
        private static string filePath;

        public static void SaveSettings()
        {
            GetFileName();
            var settings = ProgramSetting.AppSettings;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            var json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(filePath, json);
        }

        public static void LoadSettings()
        {
            try
            {
                GetFileName();
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                var setting = JsonSerializer.Deserialize<AppSettings>(fs);
                ProgramSetting.AppSettings = setting;
                RecentFilesProcessor.RemoveNotExisted();
                RecentFilesProcessor.SortRecentFiles();
            }
            catch (Exception ex)
            {
                ProgramSetting.AppSettings = new();
            }

        }

        private static void GetFileName()
        {
            var folder = AppContext.BaseDirectory;
            filePath =  Path.Combine(folder, fileName);
        }
    }
}
