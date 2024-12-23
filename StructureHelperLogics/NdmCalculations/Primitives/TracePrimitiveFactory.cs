using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Tables;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Logic for creating of trace entries of primitives
    /// </summary>
    public class TracePrimitiveFactory : ITraceEntityFactory<INdmPrimitive>
    {
        const int rowSize = 2;
        private List<ITraceLoggerEntry> traceLoggerEntries;
        public int Priority { get; set; } = LoggerService.GetPriorityByStatus(TraceLogStatuses.Info);
        public IEnumerable<INdmPrimitive>? Collection { get; set; }
        public List<ITraceLoggerEntry> GetTraceEntries()
        {
            Check();
            traceLoggerEntries = new();
            traceLoggerEntries.Add(new StringLogEntry()
            {
                Message = $"There are (is) {Collection.Count()} primitive(s)",
                Priority = Priority
            }
            );
            ProcessCollection();
            return traceLoggerEntries;
        }

        private void Check()
        {
            if (Collection is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": Collection of primitives");
            }
        }

        private void ProcessCollection()
        {
            foreach (var item in Collection)
            {
                var table = new TableLogEntry(rowSize)
                {
                    Priority = Priority
                };
                table.ColumnWidth[1] = 200;
                table.Table.AddRow(GetHeader(item));
                table.Table.AddRows(GetCommonRows(item));
                if (item is IRectangleNdmPrimitive rectangle)
                {
                    table.Table.AddRows(ProcessRectangle(rectangle.Shape as IRectangleShape));
                }
                if (item is IEllipseNdmPrimitive ellipse)
                {
                    IRectangleShape? rectShape = ellipse.Shape as IRectangleShape;
                    if (rectShape.Width == rectShape.Height)
                    {
                        table.Table.AddRows(ProcessCircle(rectShape));
                    }
                    else
                    {
                        table.Table.AddRows(ProcessRectangle(rectShape));
                    }
                }
                if (item is IPointNdmPrimitive point)
                {
                    table.Table.AddRows(ProcessPoint(point));
                }
                traceLoggerEntries.Add(table);
            }
        }

        private IEnumerable<IShTableRow<ITraceLoggerEntry>> ProcessCircle(IRectangleShape rectShape)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            ShTableRow<ITraceLoggerEntry> ndmRow;
            ndmRow = new ShTableRow<ITraceLoggerEntry>(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = "Diameter",
                Priority = Priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = rectShape.Width.ToString() + "(m)",
                Priority = Priority
            };
            rows.Add(ndmRow);
            return rows;
        }

        private List<IShTableRow<ITraceLoggerEntry>> ProcessPoint(IPointNdmPrimitive point)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            ShTableRow<ITraceLoggerEntry> ndmRow;
            ndmRow = new ShTableRow<ITraceLoggerEntry>(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = "Area",
                Priority = Priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = point.Area.ToString() + "(m^2)",
                Priority = Priority
            };
            rows.Add(ndmRow);
            return rows;
        }

        private List<IShTableRow<ITraceLoggerEntry>> GetCommonRows(INdmPrimitive ndmPrimitive)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            ShTableRow<ITraceLoggerEntry> ndmRow;
            ndmRow = new(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = "Material",
                Priority = Priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = ndmPrimitive.NdmElement.HeadMaterial.Name,
                Priority = Priority
            };
            rows.Add(ndmRow);
            ndmRow = new(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = "Center",
                Priority = Priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = "X = " + ndmPrimitive.Center.X.ToString() + "(m)" + ", Y = " + ndmPrimitive.Center.Y.ToString() + "(m)",
                Priority = Priority
            };
            rows.Add(ndmRow);
            return rows;
        }

        private List<IShTableRow<ITraceLoggerEntry>> ProcessRectangle(IRectangleShape rectShape)
        {
            List<IShTableRow<ITraceLoggerEntry>> rows = new();
            ShTableRow<ITraceLoggerEntry> ndmRow;
            ndmRow = new ShTableRow<ITraceLoggerEntry>(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = "Height",
                Priority = Priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = rectShape.Height.ToString() + "(m)",
                Priority = Priority
            };
            rows.Add(ndmRow);
            ndmRow = new ShTableRow<ITraceLoggerEntry>(rowSize);
            ndmRow.Elements[0].Value = new StringLogEntry()
            {
                Message = "Width",
                Priority = Priority
            };
            ndmRow.Elements[1].Value = new StringLogEntry()
            {
                Message = rectShape.Width.ToString() + "(m)",
                Priority = Priority
            };
            rows.Add(ndmRow);
            return rows;
        }

        private IShTableRow<ITraceLoggerEntry> GetHeader(INdmPrimitive ndmPrimitive)
        {
            const CellRole cellRole = CellRole.Header;
            string[] ColumnList = new string[]
            {
                "Primitive name",
                ndmPrimitive.Name
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
    }
}
