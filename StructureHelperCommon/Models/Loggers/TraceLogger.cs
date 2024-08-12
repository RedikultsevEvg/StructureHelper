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

        public void AddMessage(string message, TraceLogStatuses status, int shiftPrioriry)
        {
            if (status == TraceLogStatuses.Fatal) { message = $"Fatal error! {message}"; }
            if (status == TraceLogStatuses.Error) { message = $"Error! {message}"; }
            if (status == TraceLogStatuses.Warning) { message = $"Warning! {message}"; }
            TraceLoggerEntries.Add(new StringLogEntry()
            {
                Message = message,
                Priority = LoggerService.GetPriorityByStatus(status) + shiftPrioriry,
            });
        }        
        public void AddMessage(string message, int priority)
        {
            TraceLoggerEntries.Add(new StringLogEntry()
            {
                Message = message,
                Priority = priority,
            });
        }

        public void AddMessage(string message)
        {
            AddMessage(message, TraceLogStatuses.Info,0);
        }

        public void AddMessage(string message, TraceLogStatuses status)
        {
            TraceLoggerEntries.Add(new StringLogEntry()
            {
                Message = message,
                Priority = LoggerService.GetPriorityByStatus(status)
            });
        }
    }
}
