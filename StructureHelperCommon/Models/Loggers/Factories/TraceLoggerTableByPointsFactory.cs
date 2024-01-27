using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Loggers
{
    public static class TraceLoggerTableByPointsFactory
    {
        public static TableLoggerEntry GetTableByPoint2D(IPoint2D point2D)
        {
            var table = new TableLoggerEntry(2);
            table.Table.AddRow(GetHeaderRow());
            table.Table.AddRow(GetPointRow(point2D));
            return table;
        }
        public static TableLoggerEntry GetTableByPoint2D(IEnumerable<IPoint2D> points)
        {
            var table = new TableLoggerEntry(2);
            table.Table.AddRow(GetHeaderRow());
            foreach (var item in points)
            {
                table.Table.AddRow(GetPointRow(item));           
            }
            return table;
        }

        private static ShTableRow<ITraceLoggerEntry> GetHeaderRow()
        {
            var headerRow = new ShTableRow<ITraceLoggerEntry>(2);
            headerRow.Elements[0] = new StringLoggerEntry()
            {
                Message = "X",
                Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info)
            };
            headerRow.Elements[1] = new StringLoggerEntry()
            {
                Message = "Y",
                Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info)
            };
            return headerRow;
        }
        private static ShTableRow<ITraceLoggerEntry> GetPointRow(IPoint2D point2D)
        {
            var pointRow = new ShTableRow<ITraceLoggerEntry>(2);
            pointRow.Elements[0] = new StringLoggerEntry()
            {
                Message = Convert.ToString(point2D.X),
                Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info)
            };
            pointRow.Elements[1] = new StringLoggerEntry()
            {
                Message = Convert.ToString(point2D.Y),
                Priority = LoggerService.GetPriorityByStatus(TraceLoggerStatuses.Info)
            };
            return pointRow;
        }
    }
}
