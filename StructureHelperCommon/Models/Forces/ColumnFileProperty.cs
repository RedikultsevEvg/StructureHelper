using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ColumnFileProperty : IColumnFileProperty
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

        public ColumnFileProperty(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public ColumnFileProperty(string columnName) : this(Guid.NewGuid(), columnName)
        {
            
        }

        public object Clone()
        {
            var cloneLogic = new ColumnFilePropertyCloningStrategy();
            return cloneLogic.GetClone(this);
        }
    }
}
