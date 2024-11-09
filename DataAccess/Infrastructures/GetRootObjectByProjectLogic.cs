using DataAccess.DTOs;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class GetRootObjectByProjectLogic : IGetRootObjectByProjectLogic
    {
        private readonly IFileVersion version = ProgramSetting.GetCurrentFileVersion();
        private Dictionary<(Guid id, Type type), ISaveable> refDictinary = new Dictionary<(Guid id, Type type), ISaveable>();

        public IShiftTraceLogger? TraceLogger { get; set; }

        public GetRootObjectByProjectLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IProject Project { get; set; }

        public RootObjectDTO? GetRootObject()
        {
            FileVersionDTO fileVersion = GetVersionDTO();
            ProjectDTO project = GetProjectDTO();
            return new RootObjectDTO() { FileVersion = fileVersion, Project = project };
        }

        private FileVersionDTO GetVersionDTO()
        {
            FileVersionToDTOConvertStrategy fileVersionDTOConvertStrategy = new()
            {
                ReferenceDictionary = refDictinary,
                TraceLogger = TraceLogger
            };
            DictionaryConvertStrategy<FileVersionDTO, IFileVersion> dictionaryConvertStrategy = new()
            {
                ConvertStrategy = fileVersionDTOConvertStrategy,
                ReferenceDictionary = refDictinary,
                TraceLogger = TraceLogger
            };
            var versionDTO = dictionaryConvertStrategy.Convert(version);
            return versionDTO;
        }

        private ProjectDTO GetProjectDTO()
        {
            ProjectToDTOConvertStrategy convertStrategy = new()
            {
                ReferenceDictionary = refDictinary,
                TraceLogger = TraceLogger
            };
            DictionaryConvertStrategy<ProjectDTO, IProject> dictionaryConvertStrategy = new()
            {
                ConvertStrategy = convertStrategy,
                ReferenceDictionary = refDictinary,
                TraceLogger = TraceLogger
            };
            ProjectDTO projectDTO = dictionaryConvertStrategy.Convert(Project);
            return projectDTO;
        }
    }
}
