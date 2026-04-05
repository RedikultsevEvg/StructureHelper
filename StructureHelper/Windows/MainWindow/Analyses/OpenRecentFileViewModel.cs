using DataAccess.Infrastructures;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    public class OpenRecentFileViewModel : OkCancelViewModelBase
    {
        private FileLogic fileLogic;
        private RelayCommand fileOpenCommand;

        public ICommand FileOpenCommand => fileOpenCommand ??= new RelayCommand(obj => OpenFile(), b => Files.SelectedItem is not null);

        private void OpenFile()
        {
            if (Files.SelectedItem is null) { return; }
            if (Files.SelectedItem.IsFileExist == false) { return; }

            IProjectAccessLogic projectAccessLogic = new ProjectAccessLogic();
            var result = projectAccessLogic.OpenProject(Files.SelectedItem.FileName);
            if (result.IsValid == true)
            {
                result.Project.IsActual = true;
                ProgramSetting.Projects.Clear();
                ProgramSetting.Projects.Add(result.Project);
            }
            else
            {
                //ProgramSetting.Projects.Add(currentProject);
            }
            fileLogic.ShowTraceLoggerEntries();
            fileLogic.ParentVM.AnalysesLogic.Refresh();
            ParentWindow.Close();
        }

        public SelectItemVM<RecentFileViewModel> Files { get; private set; }

        public OpenRecentFileViewModel(FileLogic fileLogic)
        {
            this.fileLogic = fileLogic;
            var files = ProgramSetting.AppSettings.RecentFilesSettings.Files;
            List<RecentFileViewModel> convertedFiles = [];
            foreach (var item in files)
            {
                convertedFiles.Add(new RecentFileViewModel(item));
            }
            Files = new(convertedFiles);
        }
    }
}
