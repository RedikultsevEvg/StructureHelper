using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Tables;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    internal class TraceRebarCrackResultLogic : ITraceCollectionLogic<IRebarCrackResult>
    {
        private List<ITraceLoggerEntry> traceLoggerEntries;
        private TableLogEntry table;
        private const int resultRowSize = 4;
        private const int rebarRowSize = 2;
        private const int calctTermColumnWidth = 80;
        private const int elongationColumnWidth = 200;
        private const int psiSColumnWidth = 200;
        private const int crackWidthColumnWidth = 250;

        public IEnumerable<IRebarCrackResult>? Collection { get; set; }
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
            ProcessCollection();
            return traceLoggerEntries;
        }

        private void ProcessCollection()
        {
            var message = new StringLogEntry()
            {
                Message = "Results by rebars",
                Priority = Priority
            };
            traceLoggerEntries.Add(message);
            table = new TableLogEntry(rebarRowSize)
            {
                Priority = Priority
            };
            table.ColumnWidth[0] = 100;
            table.ColumnWidth[1] = calctTermColumnWidth + elongationColumnWidth + psiSColumnWidth + crackWidthColumnWidth;
            table.Table.AddRow(GetHeader());
            foreach (var item in Collection)
            {
                List<IShTableRow<ITraceLoggerEntry>> rows = ProcessRebar(item);
                rows.ForEach(x => table.Table.AddRow(x));
            }
            traceLoggerEntries.Add(table);
        }

        private List<IShTableRow<ITraceLoggerEntry>> ProcessRebar(IRebarCrackResult item)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            IShTableRow<ITraceLoggerEntry> rebarRow = new ShTableRow<ITraceLoggerEntry>(rebarRowSize);
            rebarRow.Elements[0].Value = new StringLogEntry()
            {
                Message = item.RebarPrimitive.Name,
                Priority = Priority
            };
            rebarRow.Elements[1].Value = GetResultTable(item);
            rows.Add(rebarRow);
            return rows;
        }

        private TableLogEntry GetResultTable(IRebarCrackResult item)
        {
            TableLogEntry resultTable = new(4) { Priority = Priority};
            resultTable.ColumnWidth[0] = calctTermColumnWidth;
            resultTable.ColumnWidth[1] = elongationColumnWidth;
            resultTable.ColumnWidth[2] = psiSColumnWidth;
            resultTable.ColumnWidth[3] = crackWidthColumnWidth;
            ShTableRow<ITraceLoggerEntry> resultRow;
            string termName = "Long term";
            CrackWidthRebarTupleResult rebarResult = item.LongTermResult;
            resultRow = GetResultCalcRow(termName, rebarResult);
            resultTable.Table.AddRow(resultRow);
            termName = "Short term";
            rebarResult = item.ShortTermResult;
            resultRow = GetResultCalcRow(termName, rebarResult);
            resultTable.Table.AddRow(resultRow);
            return resultTable;
        }
        private ShTableRow<ITraceLoggerEntry> GetResultCalcRow(string longTermName, CrackWidthRebarTupleResult rebarResult)
        {
            ShTableRow<ITraceLoggerEntry> row;
            row = new(resultRowSize);
            int priority = Priority;
            //if (item.IsValid == false)
            //{
            //    priority = LoggerService.GetPriorityByStatus(TraceLogStatuses.Error);
            //}
            row.Elements[0].Value = new StringLogEntry()
            {
                Message = longTermName,
                Priority = priority
            };
            double elongation = rebarResult.RebarStressResult.RebarStrain + rebarResult.RebarStressResult.ConcreteStrain;
            row.Elements[1].Value = new StringLogEntry()
            {
                Message = $"{rebarResult.RebarStressResult.RebarStrain} + ({rebarResult.RebarStressResult.ConcreteStrain}) = {elongation}",
                Priority = priority
            };
            row.Elements[2].Value = new StringLogEntry()
            {
                Message = $"{rebarResult.SofteningFactor}",
                Priority = priority
            };
            row.Elements[3].Value = new StringLogEntry()
            {
                Message = $" Acrc = {rebarResult.CrackWidth}(m),\nAcrc,ult=({rebarResult.UltimateCrackWidth}(m))",
                Priority = priority
            };
            return row;
        }

        private IShTableRow<ITraceLoggerEntry> GetHeader()
        {
            const CellRole cellRole = CellRole.Header;
            string[] ColumnList = new string[]
            {
                "Rebar",
                "Results"
            };
            var headerRow = new ShTableRow<ITraceLoggerEntry>(rebarRowSize);
            headerRow.Elements.ForEach(x => x.Role = cellRole);

            for (int i = 0; i < ColumnList.Length; i++)
            {
                headerRow.Elements[i].Value = new StringLogEntry()
                {
                    Message = ColumnList[i],
                    Priority = Priority
                };
            }
            TableLogEntry resultTable = new(resultRowSize);
            resultTable.ColumnWidth[0] = calctTermColumnWidth;
            resultTable.ColumnWidth[1] = elongationColumnWidth;
            resultTable.ColumnWidth[2] = psiSColumnWidth;
            resultTable.ColumnWidth[3] = crackWidthColumnWidth;
            resultTable.Table.AddRow(GetResultHeader());
            headerRow.Elements[1].Value = resultTable;
            return headerRow;
        }

        private IShTableRow<ITraceLoggerEntry> GetResultHeader()
        {
            const CellRole cellRole = CellRole.Header;
            string[] ColumnList = new string[]
            {
                "Calc term",
                "Elongation",
                "Softening factor",
                "Crack width"
            };
            var ndmRow = new ShTableRow<ITraceLoggerEntry>(resultRowSize);
            ndmRow.Elements.ForEach(x => x.Role = cellRole);

            for (int i = 0; i < resultRowSize; i++)
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
