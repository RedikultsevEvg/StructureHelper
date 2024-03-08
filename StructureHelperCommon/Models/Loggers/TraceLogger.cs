using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelperCommon.Models
{
    public class TraceLogger : ITraceLogger
    {       
        public List<ITraceLoggerEntry> TraceLoggerEntries { get; }
        public bool KeepErrorStatus { get; set; }

        public TraceLogger()
        {
            TraceLoggerEntries = new();
            KeepErrorStatus = true;
        }

        public void AddMessage(string message, TraceLogStatuses status = TraceLogStatuses.Info, int shiftPrioriry = 0)
        {
            if (status == TraceLogStatuses.Fatal) { message = $"Fatal error! {message}"; }
            if (status == TraceLogStatuses.Error) { message = $"Error! {message}"; }
            if (status == TraceLogStatuses.Warning) { message = $"Warning! {message}"; }
            TraceLoggerEntries.Add(new StringLogEntry()
            {
                Message = message,
                Priority = LoggerService.GetPriorityByStatus(status)
            });
        }        
        public void AddMessage(string message, int priority)
        {
            TraceLoggerEntries.Add(new StringLogEntry()
            {
                Message = message,
                Priority = priority
            });
        }
    }
}
