using DataAccess.Infrastructures;
using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.ProgressViews;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services.FileServices;
using StructureHelperLogics.Models.CrossSections;
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

        public AnalysesManagerViewModel ParentVM { get; set; }

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
                IsActual = true
            };
            ProgramSetting.Projects.Add(newProject);
            return newProject;
        }
        private SaveFileResult SaveAsFile()
        {
            var project = ProgramSetting.CurrentProject;
            traceLogger = new ShiftTraceLogger();
            projectAccessLogic.TraceLogger = traceLogger;
            var result = projectAccessLogic.SaveProjectAs(project);
            ShowEntries();
            return result;
        }
        public bool ExitProgram()
        {
            var collection = ProgramSetting.Projects.ToList();
            foreach (var project in collection)
            {
                if (CloseFile(project) == false)
                {
                    return false;
                };
            }
            return true;
        }
        public bool CloseFile()
        {
            var project = ProgramSetting.CurrentProject;
            if (project is null)
            {
                return false;
            }
            bool closingResult = CloseFile(project);
            return closingResult;
        }
        private bool CloseFile(IProject project)
        {
            if (project.IsActual == true)
            {
                ProgramSetting.Projects.Remove(project);
                return true;
            }
            DialogResult dialogResult = GetDialog(project);
            if (dialogResult == DialogResult.Yes)
            {
                var result = SaveFile(project);
                if (result.IsValid = false)
                {
                    return false;
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                project.IsActual = true;
                ProgramSetting.Projects.Remove(project);
                return true;
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                return false;
            }
            project.IsActual = true;
            CloseFile(project);
            return true;
        }

        private static DialogResult GetDialog(IProject project)
        {
            return MessageBox.Show($"File {project.FullFileName} has unsaved changes", $"StructureHelper: Save file?",  MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }

        private void SaveFile()
        {
            var project = ProgramSetting.CurrentProject;
            SaveFile(project);
        }
        private SaveFileResult SaveFile(IProject project)
        {
            traceLogger = new ShiftTraceLogger();
            projectAccessLogic.TraceLogger = traceLogger;
            var result = projectAccessLogic.SaveProject(project);
            ShowEntries();
            return result;
        }
        private void OpenFile()
        {
            var currentProject = ProgramSetting.CurrentProject;
            var closeResult = CloseFile();
            if (closeResult == false)
            {
                return;
            }
            OpenNewFile(currentProject);
        }

        private void OpenNewFile(IProject currentProject)
        {
            traceLogger = new ShiftTraceLogger();
            projectAccessLogic.TraceLogger = traceLogger;
            var result = projectAccessLogic.OpenProject();
            if (result.IsValid == true)
            {
                result.Project.IsActual = true;
                ProgramSetting.Projects.Clear();
                ProgramSetting.Projects.Add(result.Project);
            }
            else
            {
                ProgramSetting.Projects.Add(currentProject);
            }
            ShowEntries();
            ParentVM.AnalysesLogic.Refresh();
        }

        public void OpenNewFile(string fileName)
        {
            traceLogger = new ShiftTraceLogger();
            projectAccessLogic.TraceLogger = traceLogger;
            var result = projectAccessLogic.OpenProject(fileName);
            if (result.IsValid == true)
            {
                result.Project.IsActual = true;
                ProgramSetting.Projects.Clear();
                ProgramSetting.Projects.Add(result.Project);
            }
            else
            {
                ProgramSetting.Projects.Add(new Project());
            }
            ShowEntries();
            ParentVM.AnalysesLogic.Refresh();
        }

        private void ShowEntries()
        {
            var filteredEntries = traceLogger.TraceLoggerEntries.Where(x => x.Priority < 300);
            if (filteredEntries.Any())
            {
                var wnd = new TraceDocumentView(traceLogger);
                wnd.ShowDialog();
            }
        }
    }
}
