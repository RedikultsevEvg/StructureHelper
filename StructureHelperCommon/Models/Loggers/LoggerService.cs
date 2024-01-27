using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Loggers
{
    public static class LoggerService
    {
        const int fatal = 0;
        const int error = 100;
        const int warning = 200;
        const int info = 300;
        const int service = 400;
        const int debug = 500;
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
