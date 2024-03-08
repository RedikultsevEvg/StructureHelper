using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models
{
    public static class LoggerService
    {
        const int fatal = 0;
        const int error = 100;
        const int warning = 200;
        const int info = 300;
        const int service = 400;
        const int debug = 500;
        public static int GetPriorityByStatus(TraceLogStatuses status)
        {
            if (status == TraceLogStatuses.Fatal) { return fatal; }
            else if (status == TraceLogStatuses.Error) { return error; }
            else if (status == TraceLogStatuses.Warning) { return warning; }
            else if (status == TraceLogStatuses.Info) { return info; }
            else if (status == TraceLogStatuses.Service) { return service; }
            else if (status == TraceLogStatuses.Debug) { return debug; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(status));
            }
        }
    }
}
