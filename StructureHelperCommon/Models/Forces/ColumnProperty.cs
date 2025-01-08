using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ColumnProperty : IColumnProperty
    {
        public Guid Id { get; private set; }
        public string ColumnName { get; set; } = string.Empty;
        public int ColumnIndex { get; set; } = 0;
        public double ColumnFactor { get; set; } = 1d;
        public ColumnProperty(Guid id, string columnName)
        {
            Id = id;
            ColumnName = columnName;
        }
        public ColumnProperty(string columnName) : this(Guid.NewGuid(), columnName)
        {
            
        }
    }
}
