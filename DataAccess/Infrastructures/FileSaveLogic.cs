using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services.FileServices;

namespace DataAccess.Infrastructures
{
    public class FileSaveLogic : IFileSaveLogic
    {
        public IShiftTraceLogger? TraceLogger { get; set; }

        public SaveFileResult SaveFile(IProject project)
        {
            SaveFileResult result = new(); 
            if (project.FullFileName == string.Empty || project.FullFileName == "")
            {
                var selectResult = SelectFileName(project);
                if (selectResult.IsValid == false)
                {
                    TraceLogger?.AddMessage(selectResult.Description);
                    result.IsValid = false;
                    return result;
                }
                project.FullFileName = selectResult.FileName;
            }
            result.FileName = project.FullFileName;
            try
            {
                SaveToFile(project);
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                result.Description += ex.Message;
            }
            return result;
        }

        private SaveFileResult SelectFileName(IProject project)
        {
            FileDialogSaver saver = new();
            saver.InputData = new SaveDialogInputData()
            {
                FilterString = "StructureHelper project file (*.shpj)|*.shpj|All Files (*.*)|*.*",
                InitialDirectory = project.FullFileName,
            };
            saver.TraceLogger = TraceLogger;
            var saveResult = saver.SaveFile();
            return saveResult;
        }

        private void SaveToFile(IProject project)
        {
            ISaveProjectToFileLogic saveProjectLogic = new SaveProjectToFileLogic(TraceLogger)
            {
                Project = project
            };
            saveProjectLogic.SaveProject();
        }


        public SaveFileResult SaveFileAs(IProject project)
        {
            var tmpFullFileName = project.FullFileName;
            project.FullFileName = string.Empty;
            var result = SaveFile(project);
            if (result.IsValid == false)
            {
                project.FullFileName = tmpFullFileName;
            }
            return result;
        }
    }
}
