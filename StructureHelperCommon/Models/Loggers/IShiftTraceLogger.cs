using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Loggers
{
    public interface IShiftTraceLogger : ITraceLogger
    {
        ITraceLogger Logger { get; set; }
        int ShiftPriority { get; set; }
        void AddEntry(ITraceLoggerEntry loggerEntry);
        IShiftTraceLogger GetSimilarTraceLogger(int shftPriority = 0);
    }
}
