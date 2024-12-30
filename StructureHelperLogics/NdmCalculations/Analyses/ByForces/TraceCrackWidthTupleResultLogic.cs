using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    internal class TraceCrackWidthTupleResultLogic : ITraceCollectionLogic<ITupleCrackResult>
    {
        public IEnumerable<ITupleCrackResult>? Collection { get; set; }
        public int Priority { get; set; } = LoggerService.GetPriorityByStatus(TraceLogStatuses.Info);

        public void AddEntriesToTraceLogger(IShiftTraceLogger traceLogger)
        {
            throw new NotImplementedException();
        }

        public List<ITraceLoggerEntry> GetTraceEntries()
        {
            throw new NotImplementedException();
        }
    }
}
