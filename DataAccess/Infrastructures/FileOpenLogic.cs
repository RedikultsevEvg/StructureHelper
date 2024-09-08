using DataAccess.DTOs;
using DataAccess.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services.FileServices;

namespace DataAccess.Infrastructures
{
    public class FileOpenLogic : IFileOpenLogic
    {
        private string fileName;

        public IShiftTraceLogger? TraceLogger { get; set; }

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
            if (! File.Exists(fileName))
            {
                result.IsValid = false;
                TraceLogger?.AddMessage($"File {fileName} does not exists", TraceLogStatuses.Error);
                return result;
            }
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                        new FileVersionDTOJsonConverter(TraceLogger),  // Add the specific converter
                        // Add other converters if needed
                    },
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Include
            };
            using (StreamReader file = File.OpenText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                var fileVersion = (FileVersionDTO)serializer.Deserialize(file, typeof(FileVersionDTO));
                var checkLogic = new CheckFileVersionLogic()
                {
                    FileVersion = fileVersion,
                    TraceLogger = TraceLogger
                };
                var checkResult = checkLogic.Check();
                if (checkResult == false)
                {
                    result.IsValid = false;
                    result.Description += checkLogic.CheckResult;
                }
                else
                {
                    var currentVersion = ProgramSetting.GetCurrentFileVersion();
                    TraceLogger.AddMessage($"File version is {fileVersion.VersionNumber}.{fileVersion.SubVersionNumber}, current version is {currentVersion.VersionNumber}.{currentVersion.SubVersionNumber}");
                }
            }   
            return result;
        }
    }
}
