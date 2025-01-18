using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class CheckColumnedFilePropertyLogic : ICheckEntityLogic<IColumnedFileProperty>
    {
        private string checkResult;
        private bool result;

        public IColumnedFileProperty Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            result = true;
            int skipRows = Entity.SkipRowBeforeHeaderCount + Entity.SkipRowHeaderCount;
            if (skipRows < 0)
            {
                string errorString = ErrorStrings.DataIsInCorrect + $": skip row count must be greater or equal to 0, bu was {skipRows}";
                TraceMessage(errorString);
                return false;
            }
            if (File.Exists(Entity.FilePath) == false)
            {
                string errorString = ErrorStrings.DataIsInCorrect + $": File {Entity.FilePath} does not exist";
                TraceMessage(errorString);
                return false;
            }
            return result;
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
