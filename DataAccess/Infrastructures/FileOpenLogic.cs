using DataAccess.DTOs;
using DataAccess.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
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
        private Dictionary<(Guid id, Type type), ISaveable> refDictinary;

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
            if (!File.Exists(fileName))
            {
                result.IsValid = false;
                TraceLogger?.AddMessage($"File {fileName} does not exists", TraceLogStatuses.Error);
                return result;
            }
            string jsonData = File.ReadAllText(fileName);
            RootObjectDTO? rootObject = GetRootObject(jsonData);
            var fileVersion = rootObject.FileVersion;
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
                Project project = GetProject(rootObject);
                project.FullFileName = fileName;
                result.Project = project;
            }
            return result;
        }

        private Project GetProject(RootObjectDTO? rootObject)
        {
            var currentVersion = ProgramSetting.GetCurrentFileVersion();
            IFileVersion fileVersion = rootObject.FileVersion;
            TraceLogger?.AddMessage($"File version is {fileVersion.VersionNumber}.{fileVersion.SubVersionNumber}, current version is {currentVersion.VersionNumber}.{currentVersion.SubVersionNumber}");
            refDictinary = new Dictionary<(Guid id, Type type), ISaveable>();
            IConvertStrategy<Project, ProjectDTO> convertStrategy = new ProjectFromDTOConvertStrategy()
            {
                ReferenceDictionary = refDictinary,
                TraceLogger = TraceLogger
            };
            Project project = convertStrategy.Convert(rootObject.Project);
            return project;
        }

        private RootObjectDTO? GetRootObject(string jsonData)
        {
            JsonSerializerSettings settings = GetSettings();
            var rootObject = JsonConvert.DeserializeObject<RootObjectDTO>(jsonData, settings);
            return rootObject;
        }

        private JsonSerializerSettings GetSettings()
        {
            List<(Type type, string name)> typesNames = TypeBinderListFactory.GetTypeNameList(TypeFileVersion.version_v1);
            TypeBinder typeBinder = new(typesNames);
            var settings = new JsonSerializerSettings
            {

                Converters = new List<JsonConverter>
                {
                        new FileVersionDTOJsonConverter(TraceLogger),  // Add the specific converter
                        new ProjectDTOJsonConverter(TraceLogger)
                    },
                SerializationBinder = typeBinder,
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Include
            };
            return settings;
        }
    }
}
