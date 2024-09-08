using DataAccess.DTOs;
using DataAccess.JsonConverters;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using StructureHelperCommon.Services.FileServices;

namespace DataAccess.Infrastructures
{
    public class FileSaveLogic : IFileSaveLogic
    {
        private IFileVersion version;
        private Dictionary<(Guid id, Type type), ISaveable> refDictinary;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public void SaveFile(IProject project)
        {
            if (project.IsNewFile == true)
            {
                var result = SelectFileName(project);
                if (result.IsValid == false)
                {
                    TraceLogger?.AddMessage(result.Description);
                    return;
                }
                project.FullFileName = result.FileName;
                project.IsNewFile = false;
            }
            SaveToFile(project);
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
            version = ProgramSetting.GetCurrentFileVersion();
            refDictinary = new Dictionary<(Guid id, Type type), ISaveable>();
            FileVersionDTO versionDTO = GetVersionDTO();
            var versionString = Serialize(versionDTO, TraceLogger);
            try
            {
                File.WriteAllText(project.FullFileName, versionString);
                TraceLogger?.AddMessage($"File {project.FullFileName} was saved successfully", TraceLogStatuses.Service);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
            }
        }

        private FileVersionDTO GetVersionDTO()
        {
            FileVersionDTOConvertStrategy fileVersionDTOConvertStrategy = new()
            {
                TraceLogger = TraceLogger
            };
            DictionaryConvertStrategy<FileVersion, FileVersionDTO> dictionaryConvertStrategy = new()
            {
                ConvertStrategy = fileVersionDTOConvertStrategy,
                ReferenceDictionary = refDictinary,
                TraceLogger = TraceLogger
            };
            var versionDTO = dictionaryConvertStrategy.ConvertTo(version as FileVersion);
            return versionDTO;
        }

        private static string Serialize(object obj, IShiftTraceLogger logger)
        {
            var settings = new JsonSerializerSettings
            {            
                Converters = new List<JsonConverter>
                    {
                        new FileVersionDTOJsonConverter(logger),  // Add the specific converter
                        // Add other converters if needed
                    },
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Include
            };

            return JsonConvert.SerializeObject(obj, settings);
        }

        public void SaveFileAs(IProject project)
        {
            var tmpIsNew = project.IsNewFile;
            var tmpFullFileName = project.FullFileName;
            project.IsNewFile = true;
            SaveFile(project);
            if (project.IsNewFile == true)
            {
                project.IsNewFile = tmpIsNew;
                project.FullFileName = tmpFullFileName;
            }
        }
    }
}
