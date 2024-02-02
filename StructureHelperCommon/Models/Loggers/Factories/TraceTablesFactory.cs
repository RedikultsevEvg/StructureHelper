using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models
{
    /// <summary>
    /// Factory for creating trace table entries
    /// </summary>
    public class TraceTablesFactory
    {
        public int Priority { get; set; }
        /// <summary>
        /// Generates table entry for Point2D (2 columns, 2 rows)
        /// </summary>
        /// <param name="point2D">Point fo creating a table</param>
        /// <returns>Table entry</returns>
        public TableLogEntry GetByPoint2D(IPoint2D point2D)
        {
            var table = new TableLogEntry(2);
            table.Priority = Priority;
            table.Table.AddRow(GetPointHeaderRow());
            table.Table.AddRow(GetPointRow(point2D));
            return table;
        }
        /// <summary>
        /// Generates a table representation for the provided force tuple
        /// </summary>
        /// <param name="forceTuple">Force tuple to create the table for</param>
        /// <returns>Table entry</returns>
        public TableLogEntry GetByForceTuple(IForceTuple forceTuple)
        {
            var table = new TableLogEntry(6);
            table.Priority = Priority;
            table.Table.AddRow(GetForceTupleHeaderRow(forceTuple));
            table.Table.AddRow(GetForceTupleRow(forceTuple));
            return table;
        }
        /// <summary>
        /// Generates table entry for Point2D (2 columns, (number of poins + 1) rows)
        /// </summary>
        /// <param name="points">Collection of points for creating a table</param>
        /// <returns>Table entry</returns>
        public TableLogEntry GetByPoint2D(IEnumerable<IPoint2D> points)
        {
            var table = new TableLogEntry(2);
            table.Priority = Priority;
            table.Table.AddRow(GetPointHeaderRow());
            foreach (var item in points)
            {
                table.Table.AddRow(GetPointRow(item));           
            }
            return table;
        }
        /// <summary>
        /// Generates a table representation for the provided force tuple collection
        /// </summary>
        /// <param name="forceTuples">Force tuple collection to create the table for</param>
        /// <returns>Table entry</returns>
        public TableLogEntry GetByForceTuple(IEnumerable<IForceTuple> forceTuples)
        {
            var table = new TableLogEntry(6);
            table.Priority = Priority;
            //type of force tuple for creating a header is taken by first member
            var firstMember = forceTuples.First();
            table.Table.AddRow(GetForceTupleHeaderRow(firstMember));
            foreach (var forceTuple in forceTuples)
            {
                table.Table.AddRow(GetForceTupleRow(forceTuple));
            }
            return table;
        }
        /// <summary>
        /// Generates new trace table entry
        /// </summary>
        /// <param name="status">Default status = info</param>
        public TraceTablesFactory(TraceLogStatuses status = TraceLogStatuses.Info)
        {
            Priority = LoggerService.GetPriorityByStatus(status);
        }
        private ShTableRow<ITraceLoggerEntry> GetForceTupleHeaderRow(IForceTuple forceTuple)
        {
            const CellRole cellRole = CellRole.Header;
            string[] ColumnList = new string[] { "Mx", "My", "Nz", "Qx", "Qy", "Mz" };
            if (forceTuple is StrainTuple)
            {
                ColumnList = new string[] { "Kx", "Ky", "EpsZ", "GammaX", "GammaY", "Kz" };
            }

            var forceTupleRow = new ShTableRow<ITraceLoggerEntry>(6);
            foreach (var item in forceTupleRow.Elements)
            {
                item.Role = cellRole;
            }

            forceTupleRow.Elements[0].Value = new StringLogEntry()
            {
                Message = ColumnList[0],
                Priority = Priority
            };

            forceTupleRow.Elements[1].Value = new StringLogEntry()
            {
                Message = ColumnList[1],
                Priority = Priority
            };

            forceTupleRow.Elements[2].Value = new StringLogEntry()
            {
                Message = ColumnList[2],
                Priority = Priority
            };

            forceTupleRow.Elements[3].Value = new StringLogEntry()
            {
                Message = ColumnList[3],
                Priority = Priority
            };

            forceTupleRow.Elements[4].Value = new StringLogEntry()
            {
                Message = ColumnList[4],
                Priority = Priority
            };

            forceTupleRow.Elements[5].Value = new StringLogEntry()
            {
                Message = ColumnList[5],
                Priority = Priority
            };

            return forceTupleRow;
        }
        private ShTableRow<ITraceLoggerEntry> GetForceTupleRow(IForceTuple forceTuple)
        {
            var forceTupleRow = new ShTableRow<ITraceLoggerEntry>(6);

            forceTupleRow.Elements[0].Value = new StringLogEntry()
            {
                Message = forceTuple.Mx.ToString(),
                Priority = Priority
            };

            forceTupleRow.Elements[1].Value = new StringLogEntry()
            {
                Message = forceTuple.My.ToString(),
                Priority = Priority
            };


            forceTupleRow.Elements[2].Value = new StringLogEntry()
            {
                Message = forceTuple.Nz.ToString(),
                Priority = Priority
            };


            forceTupleRow.Elements[3].Value = new StringLogEntry()
            {
                Message = forceTuple.Qx.ToString(),
                Priority = Priority
            };


            forceTupleRow.Elements[4].Value = new StringLogEntry()
            {
                Message = forceTuple.Qy.ToString(),
                Priority = Priority
            };

            forceTupleRow.Elements[5].Value = new StringLogEntry()
            {
                Message = forceTuple.Mz.ToString(),
                Priority = Priority
            };

            return forceTupleRow;
        }

        private ShTableRow<ITraceLoggerEntry> GetPointHeaderRow()
        {
            const CellRole cellRole = CellRole.Header;

            var headerRow = new ShTableRow<ITraceLoggerEntry>(2);
            IShTableCell<ITraceLoggerEntry> tableCell;
            ITraceLoggerEntry loggerEntry;
            loggerEntry = new StringLogEntry()
            {
                Message = "X",
                Priority = Priority
            };
            tableCell = new ShTableCell<ITraceLoggerEntry>()
            {
                Value = loggerEntry,
                Role = cellRole,
            };
            headerRow.Elements[0] = tableCell;
            loggerEntry = new StringLogEntry()
            {
                Message = "Y",
                Priority = Priority
            };
            tableCell = new ShTableCell<ITraceLoggerEntry>()
            {
                Value = loggerEntry,
                Role = cellRole,
            };
            headerRow.Elements[1] = tableCell;
            return headerRow;
        }
        private ShTableRow<ITraceLoggerEntry> GetPointRow(IPoint2D point2D)
        {
            var pointRow = new ShTableRow<ITraceLoggerEntry>(2);
            pointRow.Elements[0].Value = new StringLogEntry()
            {
                Message = Convert.ToString(point2D.X),
                Priority = LoggerService.GetPriorityByStatus(TraceLogStatuses.Info)
            };
            pointRow.Elements[1].Value = new StringLogEntry()
            {
                Message = Convert.ToString(point2D.Y),
                Priority = LoggerService.GetPriorityByStatus(TraceLogStatuses.Info)
            };
            return pointRow;
        }
    }
}
