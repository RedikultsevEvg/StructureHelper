using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class SaveProjectToFileLogic : ISaveProjectToFileLogic
    {
        private IGetRootObjectByProjectLogic getRootObjectLogic;
        private IGetJsonDataByRootObjectLogic getJsonDataLogic;

        public SaveProjectToFileLogic(
            IGetRootObjectByProjectLogic getRootObjectLogic,
            IGetJsonDataByRootObjectLogic getJsonDataLogic,
            IShiftTraceLogger traceLogger)
        {
            this.getRootObjectLogic = getRootObjectLogic;
            this.getJsonDataLogic = getJsonDataLogic;
            TraceLogger = traceLogger;
        }

        public SaveProjectToFileLogic(IShiftTraceLogger traceLogger) : this (
            new GetRootObjectByProjectLogic(traceLogger),
            new GetJsonDataByRootObjectLogic(traceLogger),
            traceLogger
            )
        {
            
        }

        public IProject Project { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void SaveProject()
        {
            getRootObjectLogic.Project = Project;
            var rootObject = getRootObjectLogic.GetRootObject();
            getJsonDataLogic.RootObject = rootObject;
            var jsonData = getJsonDataLogic.GetJsonData();
            SaveStringToFile(jsonData);
            Project.IsActual = true;
        }

        private void SaveStringToFile(string jsonData)
        {
            try
            {
                File.Delete(Project.FullFileName);
                File.WriteAllText(Project.FullFileName, jsonData);
                TraceLogger?.AddMessage($"File {Project.FullFileName} was saved successfully", TraceLogStatuses.Service);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
            }
        }
    }
}
