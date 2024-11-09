using DataAccess.DTOs;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Infrastructures
{
    public class CheckRootObjectLogic : ICheckEntityLogic<IRootObjectDTO>
    {
        public IRootObjectDTO Entity { get; set; }

        public string CheckResult { get; private set; }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckRootObjectLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public bool Check()
        {
            bool result = true;
            string checkResult = string.Empty;
            var checkRootResult = CheckRootObject();
            if (checkRootResult.checkResult = false)
            {
                checkResult += checkRootResult.errorString;
                result = false;
            }
            CheckResult = checkResult;
            return result;
        }

        private (bool checkResult, string errorString) CheckRootObject()
        {
            bool checkRootResult = true;
            string errorString = string.Empty;

            if (Entity.FileVersion is null)
            {
                checkRootResult = false;
                errorString += "Section of file version is damaged";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                return (checkRootResult, errorString);
            }
            var fileVersion = Entity.FileVersion;
            var checkLogic = new CheckFileVersionLogic()
            {
                FileVersion = fileVersion,
                TraceLogger = TraceLogger
            };
            var checkResult = checkLogic.Check();
            if (checkResult == false)
            {
                checkRootResult = false;
                errorString += checkLogic.CheckResult;
                return (checkRootResult, errorString);
            }
            if (Entity.Project is null)
            {
                checkRootResult = false;
                errorString += "Section of project is damaged";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                return (checkRootResult, errorString);
            }
            return (checkRootResult, errorString);
        }
    }
}
