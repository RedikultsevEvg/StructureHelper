using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Tables;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class TraceCrackResultLogic : ITraceCollectionLogic<ITupleCrackResult>
    {
        private List<ITraceLoggerEntry> traceLoggerEntries;
        private const int rowSize = 4;

        public IEnumerable<ITupleCrackResult>? Collection { get; set; }
        public int Priority { get; set; } = LoggerService.GetPriorityByStatus(TraceLogStatuses.Info);

        public void AddEntriesToTraceLogger(IShiftTraceLogger traceLogger)
        {
            var entries = GetTraceEntries();
            entries.ForEach(x => traceLogger?.AddEntry(x));
        }

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
            foreach (var item in Collection)
            {
                int priority = Priority;
                if (item.IsValid == false)
                {
                    priority = LoggerService.GetPriorityByStatus(TraceLogStatuses.Error);
                }
                var message = new StringLogEntry()
                {
                    Message = "Force combination: " + item.InputData.TupleName,
                    Priority = priority
                };
                traceLoggerEntries.Add(message);
                if (item.IsValid == false)
                {
                    var errorMessage = new StringLogEntry()
                    {
                        Message = "Calculation is not valid: " + item.Description,
                        Priority = priority
                    };
                    traceLoggerEntries.Add(errorMessage);
                    continue;
                }
                var table = new TableLogEntry(rowSize)
                {
                    Priority = Priority
                };
                table.ColumnWidth[0] = 100;
                table.ColumnWidth[1] = 250;
                table.ColumnWidth[2] = 250;
                table.ColumnWidth[3] = 100;
                table.Table.AddRow(GetHeader(item));
                List<IShTableRow<ITraceLoggerEntry>> rows = ProcessTupleCrackResult(item);
                rows.ForEach(x => table.Table.AddRow(x));
                traceLoggerEntries.Add(table);
                if (item.IsValid == true)
                {
                    ProcessRebars(item);
                }
            }
        }

        private void ProcessRebars(ITupleCrackResult item)
        {
            ITraceEntityLogic traceEntityLogic = new TraceRebarCrackResultLogic() { Collection = item.RebarResults };
            traceLoggerEntries.AddRange(traceEntityLogic.GetTraceEntries());
        }

        private List<IShTableRow<ITraceLoggerEntry>> ProcessTupleCrackResult(ITupleCrackResult item)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            int priority = Priority;
            if (item.IsValid == false)
            {
                priority = LoggerService.GetPriorityByStatus(TraceLogStatuses.Error);
                if (item.LongTermResult is null || item.ShortTermResult is null)
                {
                    return rows;
                }
            }
            ShTableRow<ITraceLoggerEntry> row;
            string? description = item.Description;
            string termName = "Long term";
            IForceTuple? tuple = item.InputData.LongTermTuple;
            row = GetTupleRow(priority, termName, tuple, item.LongTermResult);
            rows.Add(row);
            termName = "Short term";
            tuple = item.InputData.ShortTermTuple;
            row = GetTupleRow(priority, termName, tuple, item.ShortTermResult);
            rows.Add(row);
            return rows;
        }

        private ShTableRow<ITraceLoggerEntry> GetTupleRow(int priority, string longTermName, IForceTuple? tuple, CrackWidthRebarTupleResult rebarResult)
        {
            ShTableRow<ITraceLoggerEntry> ndmRow;
            ndmRow = new(rowSize);
            string description = string.Empty;
            if (rebarResult.IsCrackLessThanUltimate == false)
            {
                description = "Actual crack width is greater than ultimate one";
                priority = LoggerService.GetPriorityByStatus(TraceLogStatuses.Error);
            }
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = longTermName,
                Priority = priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = TraceStringService.GetForces(tuple),
                Priority = priority
            };
            ndmRow.Elements[2].Value = new StringLogEntry()
            {
                Message = $" Acrc = {rebarResult.CrackWidth}(m),\nAcrc,ult=({rebarResult.UltimateCrackWidth}(m))",
                Priority = priority
            };
            ndmRow.Elements[3].Value = new StringLogEntry()
            {
                Message = description,
                Priority = priority
            };
            return ndmRow;
        }

        private IShTableRow<ITraceLoggerEntry> GetHeader(ITupleCrackResult tupleCrackResult)
        {
            const CellRole cellRole = CellRole.Header;
            string[] ColumnList = new string[]
            {
                "Calc term",
                "Forces",
                "Crack width",
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
    }
}
