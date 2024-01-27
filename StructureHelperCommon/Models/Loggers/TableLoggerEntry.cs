using StructureHelperCommon.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Loggers
{
    public class TableLoggerEntry : ITraceLoggerEntry
    {
        private ListTable<ITraceLoggerEntry> table;
        public ListTable<ITraceLoggerEntry> Table {get => table;}
        public DateTime TimeStamp { get; }

        public int Priority { get; set; }
        public TableLoggerEntry(int rowSize)
        {
            if (rowSize <= 0)
            {
                throw new ArgumentException("Row size must be greater than 0.", nameof(rowSize));
            }
            table = new(rowSize);
            TimeStamp = DateTime.Now;
        }
    }
}
