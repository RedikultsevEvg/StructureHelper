using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Analyses;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public class CheckConvertLogic<T, V> : ICheckConvertLogic<T, V> where T : ISaveable
        where V : ISaveable
    {
        private string checkResult;

        public CheckConvertLogic(IConvertStrategy<T, V> source)
        {
            ConvertStrategy = source;
            TraceLogger = source.TraceLogger;
        }

        public CheckConvertLogic()
        {
            
        }

        public IConvertStrategy<T, V> ConvertStrategy { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            checkResult = string.Empty;
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(LoggerStrings.LogicType(ConvertStrategy), TraceLogStatuses.Service);
            if (ConvertStrategy is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Convert Strategy";
                checkResult += "\n" + errorString;
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            if (ConvertStrategy.ReferenceDictionary is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Reference Dictionary";
                checkResult += "\n" + errorString;
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            TraceLogger?.AddMessage("Checking of convert strategy is ok", TraceLogStatuses.Debug);
            return true;
        }
    }
}
