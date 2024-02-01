using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models
{
    public interface ITraceLoggerEntry
    {
        DateTime TimeStamp { get;}
        int Priority { get; set; }
    }
}
