using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Loggers
{
    public class TraceLogger : ITraceLogger
    {
        const int fatal = 0;
        const int error = 1000;
        const int warning = 200;
        const int info = 300;
        const int service = 400;
        const int debug = 500;
        
        public List<ITraceLoggerEntry> TraceLoggerEntries { get; }

        public TraceLogger()
        {
            TraceLoggerEntries = new();
        }

        public void AddMessage(string message, TraceLoggerStatuses status = TraceLoggerStatuses.Info)
        {
            TraceLoggerEntries.Add(new StringLoggerEntry()
            {
                Message = message,
                Priority = GetPriorityByStatus(status)
            });
        }        
        public void AddMessage(string message, int priority)
        {
            TraceLoggerEntries.Add(new StringLoggerEntry()
            {
                Message = message,
                Priority = priority
            });
        }

        public static int GetPriorityByStatus(TraceLoggerStatuses status)
        {
            if (status == TraceLoggerStatuses.Fatal) { return fatal; }
            else if (status == TraceLoggerStatuses.Error) { return error; }
            else if (status == TraceLoggerStatuses.Warning) { return warning; }
            else if (status == TraceLoggerStatuses.Info) { return info; }
            else if (status == TraceLoggerStatuses.Service) { return service; }
            else if (status == TraceLoggerStatuses.Debug) { return debug; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(status));
            }
        }
    }
}
