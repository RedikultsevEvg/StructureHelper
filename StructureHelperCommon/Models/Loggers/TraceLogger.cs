using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelperCommon.Models.Loggers
{
    public class TraceLogger : ITraceLogger
    {       
        public List<ITraceLoggerEntry> TraceLoggerEntries { get; }

        public TraceLogger()
        {
            TraceLoggerEntries = new();
        }

        public void AddMessage(string message, TraceLoggerStatuses status = TraceLoggerStatuses.Info, int shiftPrioriry = 0)
        {
            if (status == TraceLoggerStatuses.Fatal) { message = $"Fatal error! {message}"; }
            if (status == TraceLoggerStatuses.Error) { message = $"Error! {message}"; }
            if (status == TraceLoggerStatuses.Warning) { message = $"Warning! {message}"; }
            TraceLoggerEntries.Add(new StringLoggerEntry()
            {
                Message = message,
                Priority = LoggerService.GetPriorityByStatus(status)
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
    }
}
