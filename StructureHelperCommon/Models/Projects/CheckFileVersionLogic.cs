using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace StructureHelperCommon.Models.Projects
{
    public class CheckFileVersionLogic : ICheckLogic
    {
        private const string checkIsGood = "Checking is good";

        public IFileVersion? FileVersion { get; set; }
        public string CheckResult { get; private set; } = string.Empty;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            if (FileVersion is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": File Version");
            }
            var currentVersion = ProgramSetting.GetCurrentFileVersion();
            if (currentVersion.VersionNumber < FileVersion.VersionNumber)
            {
                string message = $"File version {FileVersion.VersionNumber} is bigger than suitable version {currentVersion.VersionNumber}";
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                CheckResult += message;
                return false;
            }
            if (currentVersion.SubVersionNumber < FileVersion.SubVersionNumber)
            {
                string message = $"File version {FileVersion.VersionNumber}.{FileVersion.SubVersionNumber} is bigger than suitable version {currentVersion.VersionNumber}.{currentVersion.VersionNumber}";
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                CheckResult += message;
                return false;
            }
            CheckResult += checkIsGood;
            return true;
        }
    }
}
