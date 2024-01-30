using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Tables
{
    /// <inheritdoc/>
    public class ShTableCell<T> : IShTableCell<T>
    {
        /// <inheritdoc/>
        public T Value { get; set; }
        /// <inheritdoc/>
        public int ColumnSpan { get; set; }
        /// <inheritdoc/>
        public CellRole Role { get; set; }
        public ShTableCell()
        {
            Role = CellRole.Regular;
            ColumnSpan = 1;
        }
    }
}
