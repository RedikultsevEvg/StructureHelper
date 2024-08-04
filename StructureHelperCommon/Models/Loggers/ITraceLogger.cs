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
        void AddMessage(string message, TraceLogStatuses status, int shiftPriority = 0);
        void AddMessage(string message);
        void AddMessage(string message, int priority);
        bool KeepErrorStatus { get; set; }
    }
}
