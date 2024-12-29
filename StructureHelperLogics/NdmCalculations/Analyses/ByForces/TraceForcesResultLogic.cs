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
    public class TraceForcesResultLogic : ITraceEntityFactory<IForcesTupleResult>
    {
        const int rowSize = 4;
        private List<ITraceLoggerEntry> traceLoggerEntries;
        public IEnumerable<IForcesTupleResult>? Collection { get; set; }
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

        private IEnumerable<IShTableRow<ITraceLoggerEntry>> ProcessForceTupleResult(IForcesTupleResult item)
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
                Message = "Limit state: " + item.DesignForceTuple.LimitState.ToString() + ", Calculation term: " + item.DesignForceTuple.CalcTerm,
                Priority = priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = "Mx = " + item.DesignForceTuple.ForceTuple.Mx.ToString() + "N*m, My = " + item.DesignForceTuple.ForceTuple.My.ToString() + "N*m, Nz =" + item.DesignForceTuple.ForceTuple.Nz.ToString() + "N, ",
                Priority = priority
            };
            if (item.LoaderResults is not null)
            {
                ndmRow.Elements[2].Value = new StringLogEntry()
                {
                    Message = "Kx = " + item.LoaderResults.StrainMatrix.Kx + "(1/m), Ky = " + item.LoaderResults.StrainMatrix.Ky + "(1/m), EpsZ = " + item.LoaderResults.StrainMatrix.EpsZ + "(dimensionless)",
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

        private void Check()
        {
            if (Collection is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": Collection of primitives");
            }
        }
    }
}
