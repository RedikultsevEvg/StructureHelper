using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ColumnProperty : IColumnProperty
    {
        /// <inheritdoc/>
        public Guid Id { get; private set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string SearchingName { get; set; } = string.Empty;
        /// <inheritdoc/>
        public int Index { get; set; } = 0;
        /// <inheritdoc/>
        public double Factor { get; set; } = 1d;

        public ColumnProperty(Guid id, string columnName)
        {
            Id = id;
            Name = columnName;
        }
        public ColumnProperty(string columnName) : this(Guid.NewGuid(), columnName)
        {
            
        }

        public object Clone()
        {
            var cloneLogic = new ColumnPropertyCloningStrategy();
            return cloneLogic.GetClone(this);
        }
    }
}
