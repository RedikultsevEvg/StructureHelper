using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services
{
    internal static class TraceService
    {
        public static void TraceNdmCollection(ITraceLogger traceLogger, IEnumerable<INdm> ndmCollection)
        {
            if (traceLogger is null)
            {
                throw new StructureHelperException(ErrorStrings.NullReference + ": trace logger");
            }
            if (ndmCollection is null)
            {
                traceLogger.AddMessage(string.Intern("Collection of elementary parts is null"), TraceLogStatuses.Error);
                throw new StructureHelperException(ErrorStrings.NullReference + ": collection of elementary parts");
            }
            if (!ndmCollection.Any())
            {
                traceLogger.AddMessage("Collection of elementary parts is empty", TraceLogStatuses.Warning);
            }
            traceLogger.AddMessage(string.Format("Collection of elementary parts contains {0} parts", ndmCollection.Count()));
            var mes = "area of elementary part collection ";
            traceLogger.AddMessage(string.Format("{0} {1} A = {2}, {0} reduced {1} Ared = {3}",
                LoggerStrings.Summary,
                mes,
                ndmCollection.Sum(x => x.Area),
                ndmCollection.Sum(x => x.Area * x.StressScale)),
                TraceLogStatuses.Service);
            traceLogger.AddMessage($"Minimum x = {ndmCollection.Min(x => x.CenterX)}", TraceLogStatuses.Debug);
            traceLogger.AddMessage($"Maximum x = {ndmCollection.Max(x => x.CenterX)}", TraceLogStatuses.Debug);
            traceLogger.AddMessage($"Minimum y = {ndmCollection.Min(x => x.CenterY)}", TraceLogStatuses.Debug);
            traceLogger.AddMessage($"Maximum y = {ndmCollection.Max(x => x.CenterY)}", TraceLogStatuses.Debug);
        }
    }
}
