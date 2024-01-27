using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Loggers
{
    public class TraceTablesFactory
    {
        public int Priority { get; set; }
        public TraceTablesFactory(TraceLoggerStatuses status = TraceLoggerStatuses.Info, int priorityShift = 0)
        {
            Priority = LoggerService.GetPriorityByStatus(status) + priorityShift;
        }
        public TableLoggerEntry GetTableByPoint2D(IPoint2D point2D)
        {
            var table = new TableLoggerEntry(2);
            table.Priority = Priority;
            table.Table.AddRow(GetHeaderRow());
            table.Table.AddRow(GetPointRow(point2D));
            return table;
        }
        public TableLoggerEntry GetTableByPoint2D(IEnumerable<IPoint2D> points)
        {
            var table = new TableLoggerEntry(2);
            table.Priority = Priority;
            table.Table.AddRow(GetHeaderRow());
            foreach (var item in points)
            {
                table.Table.AddRow(GetPointRow(item));           
            }
            return table;
        }

        private ShTableRow<ITraceLoggerEntry> GetHeaderRow()
        {
            var headerRow = new ShTableRow<ITraceLoggerEntry>(2);
            IShTableCell<ITraceLoggerEntry> tableCell;
            ITraceLoggerEntry loggerEntry;
            loggerEntry = new StringLoggerEntry()
            {
                Message = "X",
                Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info)
            };
            tableCell = new ShTableCell<ITraceLoggerEntry>()
            {
                Value = loggerEntry,
                Role = CellRole.Header,
            };
            headerRow.Elements[0] = tableCell;
            loggerEntry = new StringLoggerEntry()
            {
                Message = "Y",
                Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info)
            };
            tableCell = new ShTableCell<ITraceLoggerEntry>()
            {
                Value = loggerEntry,
                Role = CellRole.Header,
            };
            headerRow.Elements[1] = tableCell;
            return headerRow;
        }
        private ShTableRow<ITraceLoggerEntry> GetPointRow(IPoint2D point2D)
        {
            var pointRow = new ShTableRow<ITraceLoggerEntry>(2);
            pointRow.Elements[0].Value = new StringLoggerEntry()
            {
                Message = Convert.ToString(point2D.X),
                Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info)
            };
            pointRow.Elements[1].Value = new StringLoggerEntry()
            {
                Message = Convert.ToString(point2D.Y),
                Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info)
            };
            return pointRow;
        }
    }
}
