using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Tables;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Provides trace logger inforvation for collection of force tuple results
    /// </summary>
    public class TraceForcesResultLogic : ITraceCollectionLogic<IExtendedForceTupleCalculatorResult>
    {
        const int rowSize = 4;
        private List<ITraceLoggerEntry> traceLoggerEntries;
        public IEnumerable<IExtendedForceTupleCalculatorResult>? Collection { get; set; }
        public int Priority { get; set; } = LoggerService.GetPriorityByStatus(TraceLogStatuses.Info);

        public List<ITraceLoggerEntry> GetTraceEntries()
        {
            Check();
            traceLoggerEntries = new();
            traceLoggerEntries.Add(new StringLogEntry()
            {
                Message = $"There are totally {Collection.Count()} result(s), {Collection.Count(x => x.IsValid == true)} valid result(s), {Collection.Count(x => x.IsValid == false)} invalid result(s)",
                Priority = Priority
            }
            );
            ProcessCollection();
            return traceLoggerEntries;
        }

        private void ProcessCollection()
        {
            var table = new TableLogEntry(rowSize)
            {
                Priority = Priority
            };
            table.ColumnWidth[2] = 250;
            table.ColumnWidth[3] = 250;
            table.Table.AddRow(GetHeader());
            foreach (var item in Collection)
            {
                table.Table.AddRows(ProcessForceTupleResult(item));
            }
            traceLoggerEntries.Add(table);
        }

        private IEnumerable<IShTableRow<ITraceLoggerEntry>> ProcessForceTupleResult(IExtendedForceTupleCalculatorResult item)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            int priority = Priority;
            if (item.IsValid == false)
            {
                priority = LoggerService.GetPriorityByStatus(TraceLogStatuses.Error);
            }
            ShTableRow<ITraceLoggerEntry> ndmRow;
            ndmRow = new(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = TraceStringService.GetLimitStateAndCalctTerm(item.StateCalcTermPair),
                Priority = priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = TraceStringService.GetForces(item.ForcesTupleResult.ForceTuple),
                Priority = priority
            };
            if (item.ForcesTupleResult.LoaderResults is not null)
            {
                ndmRow.Elements[2].Value = new StringLogEntry()
                {
                    Message = TraceStringService.GetCuvatures(item.ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix),
                    Priority = priority
                };
            }
            ndmRow.Elements[3].Value = new StringLogEntry()
            {
                Message = item.Description,
                Priority = priority
            };
            rows.Add(ndmRow);
            return rows;
        }

        private IShTableRow<ITraceLoggerEntry> GetHeader()
        {
            const CellRole cellRole = CellRole.Header;
            string[] ColumnList = new string[]
            {
                "Conditions",
                "Forces",
                "Curvatures",
                "Description"
            };
            var ndmRow = new ShTableRow<ITraceLoggerEntry>(rowSize);
            ndmRow.Elements.ForEach(x => x.Role = cellRole);

            for (int i = 0; i < rowSize; i++)
            {
                ndmRow.Elements[i].Value = new StringLogEntry()
                {
                    Message = ColumnList[i],
                    Priority = Priority
                };
            }
            return ndmRow;
        }

        private void Check()
        {
            if (Collection is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": Collection of primitives");
            }
        }

        public void AddEntriesToTraceLogger(IShiftTraceLogger traceLogger)
        {
            var entries = GetTraceEntries();
            entries.ForEach(x => traceLogger?.AddEntry(x));
        }
    }
}
