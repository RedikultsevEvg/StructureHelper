using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public static class RecentFilesProcessor
    {
        public static void AddFileToList(string fileName)
        {
            var settings = ProgramSetting.AppSettings.RecentFilesSettings;
            List<RecentFileSettings> files = settings.Files;
            if (files.Count == settings.MaxCount)
            {
                files.Remove(settings.Files[^1]);
            }

            var existingItem = files.Where(x => x.FileName == fileName).ToList();
            foreach (var item in existingItem)
            {
                files.Remove(item);
            }

            var file = new RecentFileSettings()
            {
                FileName = fileName,
                RecentOpeningTime = DateTime.Now,
            };
            files.Add(file);
            SortRecentFiles();
        }

        public static void SortRecentFiles()
        {
            var settings = ProgramSetting.AppSettings.RecentFilesSettings; 
            var files = settings.Files;
            var sortedFileList = files
                .OrderByDescending(x => x.RecentOpeningTime)
                .ToList();
            settings.Files = sortedFileList;
            ProgramSetting.AppSettings.RecentFilesSettings = settings;
        }

        public static void RemoveNotExisted()
        {
            var settings = ProgramSetting.AppSettings.RecentFilesSettings; 
            var files = settings.Files;
            List<RecentFileSettings> existedFiles = files
                .Where(x => File.Exists(x.FileName) == true).ToList();
            settings.Files = existedFiles;
        }
    }
}
