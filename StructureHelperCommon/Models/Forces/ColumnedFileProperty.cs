using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ColumnedFileProperty : IColumnedFileProperty
    {
        private IUpdateStrategy<IColumnedFileProperty> updateStrategy;
        /// <inheritdoc/>
        public Guid Id { get; private set; }
        /// <inheritdoc/>
        public string FilePath { get; set; } = string.Empty;
        /// <inheritdoc/>
        public int SkipRowBeforeHeaderCount { get; set; } = 2;
        /// <inheritdoc/>
        public int SkipRowHeaderCount { get; set; } = 1;
        /// <inheritdoc/>
        public double GlobalFactor { get; set; } = 1d;
        /// <inheritdoc/>
        public List<IColumnFileProperty> ColumnProperties { get; } = new();

        public ColumnedFileProperty(Guid id)
        {
            Id = id;
        }
        public ColumnedFileProperty() : this (Guid.NewGuid())
        {
            
        }

        public object Clone()
        {
            updateStrategy ??= new ColumnedFilePropertyUpdateStrategy();
            ColumnedFileProperty newItem = new();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
