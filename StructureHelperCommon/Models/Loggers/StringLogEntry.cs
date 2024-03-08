using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models
{
    public class StringLogEntry : ITraceLoggerEntry
    {
        public DateTime TimeStamp { get;}
        public string Message { get; set; }
        public int Priority { get; set; }

        public StringLogEntry()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
