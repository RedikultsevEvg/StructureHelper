using DataAccess.DTOs;
using DataAccess.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services.FileServices;
using System.Reflection.Emit;

namespace DataAccess.Infrastructures
{
    public class FileOpenLogic : IFileOpenLogic
    {
        private string fileName;
        private IGetProjectLogic getProjectLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public OpenProjectResult OpenFile()
        {
            var result = new OpenProjectResult()
            {
                IsValid = true
            };
            if (GetFilePath() == false)
            {
                result.IsValid = false;
                return result;
            }
            return OpenFile(fileName);
        }

        public OpenProjectResult OpenFile(string fileName)
        {
            var result = new OpenProjectResult()
            {
                IsValid = true
            };
            if (!File.Exists(fileName))
            {
                result.IsValid = false;
                TraceLogger?.AddMessage($"File {fileName} does not exists", TraceLogStatuses.Error);
                return result;
            }
            getProjectLogic = new GetProjectLogic(TraceLogger)
            {
                TraceLogger = TraceLogger,
                FileName = fileName
            };

            RecentFilesProcessor.AddFileToList(fileName);

            return getProjectLogic.GetProject();
        }

        private bool GetFilePath()
        {
            var inputData = new OpenFileInputData()
            {
                FilterString = "StructureHelper project file (*.shpj)|*.shpj|All Files (*.*)|*.*",
                TraceLogger = TraceLogger
            };
            var fileDialog = new FileOpener(inputData);
            var fileDialogResult = fileDialog.OpenFile();
            if (fileDialogResult.IsValid != true)
            {
                return false;
            }
            fileName = fileDialogResult.FilePath;
            return true;
        }


    }
}
