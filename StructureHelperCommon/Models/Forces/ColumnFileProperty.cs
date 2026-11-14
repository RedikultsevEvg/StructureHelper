using StructureHelperCommon.Infrastructures.Enums;
using System;

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

        public ColumnFileProperty(Guid id)
            : this(id, string.Empty)
        {
        }

        public ColumnFileProperty(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public ColumnFileProperty(string columnName) : this(Guid.NewGuid(), columnName)
        {
            
        }

        public IColumnFileProperty GetClone(IColumnFileProperty sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            var cloneLogic = new ColumnFilePropertyCloningStrategy();
            return cloneLogic.GetClone(this, cloneMode);
        }
    }
}
