using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models
{
    public interface ITraceLogger
    {
        List<ITraceLoggerEntry> TraceLoggerEntries { get; }
        void AddMessage(string message, TraceLogStatuses status = TraceLogStatuses.Info, int shiftPriority = 0);
        void AddMessage(string message, int priority);
    }
}
