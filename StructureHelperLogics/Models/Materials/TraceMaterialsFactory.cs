using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Tables;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace StructureHelperLogics.Models.Materials
{
    public class TraceMaterialsFactory : ITraceEntityFactory<IHeadMaterial>
    {
        const int rowSize = 2;
        private List<ITraceLoggerEntry> traceLoggerEntries;
        public IEnumerable<IHeadMaterial>? Collection { get; set; }
        public int Priority { get; set; } = LoggerService.GetPriorityByStatus(TraceLogStatuses.Info);

        public List<ITraceLoggerEntry> GetTraceEntries()
        {
            Check();
            traceLoggerEntries = new();
            traceLoggerEntries.Add(new StringLogEntry()
            {
                Message = $"There are (is) {Collection.Count()} distinct material(s)",
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
            table.ColumnWidth[1] = 400;
            foreach (var item in Collection)
            {
                table.Table.AddRow(GetHeader(item));
                table.Table.AddRows(GetCommonRows(item));
            }
            traceLoggerEntries.Add(table);
        }

        private void Check()
        {
            if (Collection is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": Collection of primitives");
            }
        }

        private IShTableRow<ITraceLoggerEntry> GetHeader(IHeadMaterial headMaterial)
        {
            const CellRole cellRole = CellRole.Header;
            string[] ColumnList = new string[]
            {
                "Material name",
                headMaterial.Name
            };

            var ndmRow = new ShTableRow<ITraceLoggerEntry>(rowSize);
            foreach (var item in ndmRow.Elements)
            {
                item.Role = cellRole;
            }

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

        private List<IShTableRow<ITraceLoggerEntry>> GetCommonRows(IHeadMaterial headMaterial)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            rows.AddRange(ProcessSafetyFactors(headMaterial.HelperMaterial.SafetyFactors));
            return rows;
        }

        private List<IShTableRow<ITraceLoggerEntry>> ProcessSafetyFactors(IEnumerable<IMaterialSafetyFactor> safetyFactors)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            foreach (var item in safetyFactors)
            {
                if (item.Take == false) { continue; }
                ShTableRow<ITraceLoggerEntry> ndmRow;
                ndmRow = new(rowSize);
                ndmRow.Elements[0].Value = new StringLogEntry()
                {
                    Message = "Factor name",
                    Priority = Priority
                };
                ndmRow.Elements[1].Value = new StringLogEntry()
                {
                    Message = item.Name,
                    Priority = Priority
                };
                rows.Add(ndmRow);
                ndmRow = new(rowSize);
                ndmRow.Elements[0].Value = new StringLogEntry()
                {
                    Message = "Factor description",
                    Priority = Priority
                };
                ndmRow.Elements[1].Value = new StringLogEntry()
                {
                    Message = item.Description,
                    Priority = Priority
                };
                rows.Add(ndmRow);
                ndmRow = new(rowSize);
                ndmRow.Elements[0].Value = new StringLogEntry()
                {
                    Message = "Partial factors",
                    Priority = Priority
                };
                var table = new TableLogEntry(rowSize)
                {
                    Priority = Priority
                };
                table.ColumnWidth[0] = 150;
                table.ColumnWidth[1] = 250;
                table.Table.AddRows(ProcessPartialFactors(item.PartialFactors));
                ndmRow.Elements[1].Value = table;
                rows.Add(ndmRow);
            }
            return rows;
        }

        private List<IShTableRow<ITraceLoggerEntry>> ProcessPartialFactors(IEnumerable<IMaterialPartialFactor> partialFactors)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            foreach (var item in partialFactors)
            {
                rows.AddRange(ProcessPartialFactor(item));
            }
            return rows;
        }

        private List<IShTableRow<ITraceLoggerEntry>> ProcessPartialFactor(IMaterialPartialFactor item)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            ShTableRow<ITraceLoggerEntry> ndmRow = new(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = "Factor conditions",
                Priority = Priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = "Limit state: " + item.LimitState.ToString() + ", Calculation term: " + item.CalcTerm + ", Stress state: " + item.StressState,
                Priority = Priority
            };
            rows.Add(ndmRow);
            ndmRow = new(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = "Factor value",
                Priority = Priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = item.FactorValue.ToString(),
                Priority = Priority
            };
            rows.Add(ndmRow);
            return rows;
        }
    }
}
