using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Loggers
{
    public class StringLoggerEntry : ITraceLoggerEntry
    {
        public DateTime TimeStamp { get;}
        public string Message { get; set; }
        public int Priority { get; set; }

        public StringLoggerEntry()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
