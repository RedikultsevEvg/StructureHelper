using DataAccess.Infrastructures;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.ProgressViews;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace StructureHelper.Windows.MainWindow
{
    public class FileLogic : ViewModelBase
    {
        private ICommand fileOpen;
        private ICommand fileSave;
        private IShiftTraceLogger traceLogger;

        private RelayCommand fileNew;
        private RelayCommand fileClose;
        private IProjectAccessLogic projectAccessLogic;
        private RelayCommand fileSaveAs;

        public FileLogic(IProjectAccessLogic projectAccessLogic)
        {
            this.projectAccessLogic = projectAccessLogic;
        }

        public FileLogic() : this(new ProjectAccessLogic())
        {
            
        }

        public ICommand FileOpen => fileOpen ??= new RelayCommand(obj => OpenFile());
        public ICommand FileNew => fileNew ??= new RelayCommand(obj => NewFile());
        public ICommand ProgramExit => fileNew ??= new RelayCommand(obj => ExitProgram());
        public ICommand FileClose => fileClose ??= new RelayCommand(obj => CloseFile());
        public ICommand FileSave => fileSave ??= new RelayCommand(obj => SaveFile());
        public ICommand FileSaveAs => fileSaveAs ??= new RelayCommand(obj => SaveAsFile());

        public void NewFile()
        {
            var closeResult = CloseFile();
            if (closeResult == false)
            {
                return;
            }
            var newProject = CreateNewFile();
            ProgramSetting.Projects.Add(newProject);
        }
        public IProject CreateNewFile()
        {
            var newProject = new Project()
            {
                IsNewFile = true,
                IsActual = true
            };
            ProgramSetting.Projects.Add(newProject);
            return newProject;
        }
        private void SaveAsFile()
        {
            var project = ProgramSetting.CurrentProject;
            traceLogger = new ShiftTraceLogger();
            projectAccessLogic.TraceLogger = traceLogger;
            projectAccessLogic.SaveProjectAs(project);
            ShowEntries();
        }
        private void ExitProgram()
        {
            foreach (var project in ProgramSetting.Projects)
            {
                CloseFile(project);
            }
        }
        private bool CloseFile()
        {
            var project = ProgramSetting.CurrentProject;
            if (project is null) { return false; }
            return CloseFile(project);
        }
        private bool CloseFile(IProject project)
        {
            if (project.IsActual == true & project.IsNewFile == false)
            {
                ProgramSetting.Projects.Remove(project);
                return true;
            }
            var dialogResult = MessageBox.Show($"Save file?", $"File {project.FullFileName} is not saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                SaveFile(project);
            }
            if (dialogResult == DialogResult.Cancel)
            {
                return false;
            }
            project.IsActual = true;
            project.IsNewFile = false;
            CloseFile(project);
            return true;
        }
        private void SaveFile()
        {
            var project = ProgramSetting.CurrentProject;
            SaveFile(project);
        }
        private void SaveFile(IProject project)
        {
            traceLogger = new ShiftTraceLogger();
            projectAccessLogic.TraceLogger = traceLogger;
            projectAccessLogic.SaveProject(project);
            ShowEntries();
        }
        private void OpenFile()
        {
            var currentProject = ProgramSetting.CurrentProject;
            var closeResult = CloseFile();
            if (closeResult == false)
            {
                return;
            }
            traceLogger = new ShiftTraceLogger();
            projectAccessLogic.TraceLogger = traceLogger;
            var result = projectAccessLogic.OpenProject();
            if (result.IsValid == true)
            {
                ProgramSetting.Projects.Add(result.Project);
            }
            else
            {
                ProgramSetting.Projects.Add(currentProject);
            }
            ShowEntries();
        }
        private void ShowEntries()
        {
            var filteredEntries = traceLogger.TraceLoggerEntries.Where(x => x.Priority <= 300);
            if (filteredEntries.Any())
            {
                var wnd = new TraceDocumentView(traceLogger);
                wnd.ShowDialog();
            }
        }
    }
}
