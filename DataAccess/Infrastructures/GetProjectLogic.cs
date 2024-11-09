using DataAccess.DTOs;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class GetProjectLogic : IGetProjectLogic
    {
        private IGetRootObjectByJsonDataLogic getRootObjectLogic;
        private RootObjectDTO? rootObject;
        private ICheckEntityLogic<IRootObjectDTO> checkEntityLogic;
        private OpenProjectResult result;

        public GetProjectLogic(IGetRootObjectByJsonDataLogic getRootObjectLogic,
            ICheckEntityLogic<IRootObjectDTO> checkEntityLogic,
            IShiftTraceLogger traceLogger)
        {
            this.getRootObjectLogic = getRootObjectLogic;
            this.checkEntityLogic = checkEntityLogic;
            TraceLogger = traceLogger;
        }

        public GetProjectLogic(IShiftTraceLogger traceLogger) : this (
            new GetRootObjectLogic(traceLogger),
            new CheckRootObjectLogic(traceLogger),
            traceLogger)
        {
            
        }

        public string FileName { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public OpenProjectResult GetProject()
        {
            result = new OpenProjectResult()
            {
                IsValid = true
            };
            string jsonData = File.ReadAllText(FileName);
            getRootObjectLogic.JsonData = jsonData;
            rootObject = getRootObjectLogic.GetRootObject();
            checkEntityLogic.Entity = rootObject;
            var checkResult = checkEntityLogic.Check();
            if (checkResult == false)
            {
                TraceLogger?.AddMessage(checkEntityLogic.CheckResult, TraceLogStatuses.Error);
                result.Description += checkEntityLogic.CheckResult;
                result.IsValid = false;
                return result;
            }
            Project project = GetProjectByRootObject(rootObject);
            project.FullFileName = FileName;
            result.Project = project;
            return result;
        }

        

        private Project GetProjectByRootObject(RootObjectDTO? rootObject)
        {
            var currentVersion = ProgramSetting.GetCurrentFileVersion();
            IFileVersion fileVersion = rootObject.FileVersion;
            TraceLogger?.AddMessage($"File version is {fileVersion.VersionNumber}.{fileVersion.SubVersionNumber}, current version is {currentVersion.VersionNumber}.{currentVersion.SubVersionNumber}");
            Dictionary<(Guid id, Type type), ISaveable> refDictinary = new Dictionary<(Guid id, Type type), ISaveable>();
            IConvertStrategy<Project, ProjectDTO> convertStrategy = new ProjectFromDTOConvertStrategy(refDictinary, TraceLogger);
            Project project = convertStrategy.Convert(rootObject.Project);
            return project;
        }
    }
}
