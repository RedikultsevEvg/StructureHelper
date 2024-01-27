using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Loggers
{
    public interface ITraceLogger
    {
        List<ITraceLoggerEntry> TraceLoggerEntries { get; }
        void AddMessage(string message, TraceLoggerStatuses status = TraceLoggerStatuses.Info, int shiftPriority = 0);
        void AddMessage(string message, int priority);
    }
}
