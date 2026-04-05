using StructureHelperCommon.Infrastructures.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    public class RecentFileViewModel
    {
        public string FileName { get; }
        public DateTime RecentOpeningTime { get; }

        public bool IsFileExist { get; }

        public RecentFileViewModel(RecentFileSettings fileSettings)
        {
            FileName = fileSettings.FileName;
            RecentOpeningTime = fileSettings.RecentOpeningTime;
            IsFileExist = File.Exists(FileName);
        }
    }
}
